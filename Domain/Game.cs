namespace BlazorTetris.Domain;

public sealed class Game : IDisposable
{
    public delegate void StateChangedEventHandler();
    public event StateChangedEventHandler? OnStateChanged;
    
    private CancellationTokenSource _gameLoopCancellation = new ();
    private List<Tetromino> _tetrominos = new();
    private Tetromino _currentTetromino = null!;
    private int _width;
    private int _currentPosition;
    private int _currentRotation;
    private bool _isInitialized;

    public List<Square> Squares { get; } = new();

    public void Initialize()
    {
        _width = 10;
        _currentPosition = 4;
        _currentRotation = 0;
        
        _tetrominos = new List<Tetromino>
        {
            Tetromino.L(_width), 
            Tetromino.Z(_width), 
            Tetromino.T(_width), 
            Tetromino.O(_width), 
            Tetromino.I(_width)
        };
        
        var squares = Enumerable.Range(0, 200)
            .Select(index => new Square{ Index = index})
            .ToList();

        var taken = Enumerable.Range(200, 10)
            .Select(index => new Square{ Index = index, IsTaken = true})
            .ToList();
        
        Squares.AddRange(squares);
        Squares.AddRange(taken);
        
        _currentTetromino = RandomTetromino();

        _isInitialized = true;
    }

    public void Draw()
    {
        if (!_isInitialized) return;
        
        DrawTetromino();

        _ =  new Action(MoveDownTetromino).LoopAsync(1000, _gameLoopCancellation.Token);
    }
  
    private void MoveDownTetromino()
    {
        UndrawTetromino();
        _currentPosition += _width;
        DrawTetromino();
        
        // collision detection: check if tetromino at current position hits a taken square ahead
        var anyTakenSquaresAhead = _currentTetromino
            .GetIndizes()
            .Select(index => Squares[_currentPosition + index + _width])
            .Any(square => square.IsTaken);

        if (!anyTakenSquaresAhead) return;
        
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
    
    private void DrawTetromino()
    {
        _currentTetromino
            .GetIndizes()
            .Select(index => Squares[_currentPosition + index])
            .ToList()
            .ForEach(square => square.IsTetromino = true);
        
        OnStateChanged?.Invoke();
    }

    private void UndrawTetromino()
    {
        _currentTetromino
            .GetIndizes()
            .Select(index => Squares[_currentPosition + index])
            .ToList()
            .ForEach(square => square.IsTetromino = false);
        
        OnStateChanged?.Invoke();
    }
    
    private Tetromino RandomTetromino()
    {
        var random = new Random();
        return _tetrominos[random.Next(0, _tetrominos.Count)];
    }
    
    public void Dispose()
    {
        try
        {
            _gameLoopCancellation.Cancel();
        }
        catch (Exception)
        {
        }
    }
}