using BlazorTetris.Domain;
using BlazorTetris.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorTetris.Shared;


public class Square
{
    public int Index { get; set; }
    public bool IsTetromino { get; set; }
    public bool IsTaken { get; set; }
}


public partial class Tetris : IDisposable
{
   // [Inject] public ITetrisJs Js { get; set; } = null!;
    
    private List<Square> Squares { get;  } = new();
    
    private const int width = 10;
    
    private readonly List<Tetromino> _tetrominos = new List<Tetromino>
    {
        Tetromino.L(width), Tetromino.Z(width), Tetromino.T(width), Tetromino.O(width), Tetromino.I(width)
    };
    
    private CancellationTokenSource _gameLoopCancellation = new ();
    private Tetromino _currentTetromino;
    private int _currentPosition = 4;
    private int _currentRotation = 0;
    
    protected override void OnInitialized()
    {
        var squares = Enumerable.Range(0, 200)
            .Select(index => new Square{ Index = index})
            .ToList();

        var taken = Enumerable.Range(200, 10)
            .Select(index => new Square{ Index = index, IsTaken = true})
            .ToList();
        
        Squares.AddRange(squares);
        Squares.AddRange(taken);
        
        _currentTetromino = RandomTetromino();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender) return;
        
        //await Js.ImportAsync();

        DrawTetromino(); 
        
        _ = SetIntervalAsync(MoveDownTetromino, 1000, _gameLoopCancellation.Token);
    }

        // todo: make _currentTretromi a parameter of all functions?
    
    private void MoveDownTetromino()
    {
        UndrawTetromino();
        _currentPosition += width;
        DrawTetromino();
        
        // collision detection: check if tetromino at current position hits a taken square ahead
        var anyTakenSquaresAhead = _currentTetromino
            .GetIndizes()
            .Select(index => Squares[_currentPosition + index + width])
            .Any(square => square.IsTaken);

        if (anyTakenSquaresAhead)
        {
            // mark squares of current tetromino as taken
            _currentTetromino
                .GetIndizes()
                .Select(index => Squares[_currentPosition + index])
                .ToList()
                .ForEach(square => square.IsTaken = true);
            
            // start a new tetromino falling
            _currentTetromino = RandomTetromino();
            _currentPosition = 4;
            DrawTetromino();
        }
    }
    
    private void DrawTetromino()
    {
        _currentTetromino
            .GetIndizes()
            .Select(index => Squares[_currentPosition + index])
            .ToList()
            .ForEach(square => square.IsTetromino = true);
        
        StateHasChanged();
    }

    private void UndrawTetromino()
    {
        _currentTetromino
            .GetIndizes()
            .Select(index => Squares[_currentPosition + index])
            .ToList()
            .ForEach(square => square.IsTetromino = false);
        
        StateHasChanged();
    }
    
    private Tetromino RandomTetromino()
    {
        var random = new Random();
        return _tetrominos[random.Next(0, _tetrominos.Count)];
    }
    
    private Task SetIntervalAsync(Action action, int timeout, CancellationToken cancellationToken)
    {
        try
        {
            return Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(timeout, cancellationToken);
                    action();
                }
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Task cancelled
            return Task.CompletedTask;
        }
    }

    public void Dispose()
    {
        try
        {
            _gameLoopCancellation.Cancel();
        }
        catch (ObjectDisposedException)
        {
        }
    }
}