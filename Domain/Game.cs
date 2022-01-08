namespace BlazorTetris.Domain;

// todo: extract classes

public static class Extensions
{
    public static Task LoopAsync(this Action action, int timeout, CancellationToken cancellationToken)
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
}

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

        // todo: extract methods
        
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

public class Square
{
    public int Index { get; set; }
    public bool IsTetromino { get; set; }
    public bool IsTaken { get; set; }
}

public record Rotation(int I1, int I2, int I3, int I4);

public enum RotationType { Up, Right, Down, Left }

public abstract record Tetromino
{
    public Rotation Rotation { get; private init; } = null!;
    protected int Width { get; set; }

    protected abstract Rotation Up { get; }
    protected abstract Rotation Right { get; }
    protected abstract Rotation Down { get; }
    protected abstract Rotation Left { get; }
    
    public static Tetromino L(int width, RotationType rotation = RotationType.Up) => LTetromino.Of(width, rotation);
    public static Tetromino Z(int width, RotationType rotation = RotationType.Up) => ZTetromino.Of(width, rotation);
    public static Tetromino T(int width, RotationType rotation = RotationType.Up) => TTetromino.Of(width, rotation);
    public static Tetromino O(int width, RotationType rotation = RotationType.Up) => OTetromino.Of(width, rotation);
    public static Tetromino I(int width, RotationType rotation = RotationType.Up) => ITetromino.Of(width, rotation);
    
    public Tetromino Rotate(RotationType rotation)
    {
        return rotation switch
        {
            RotationType.Up => this with { Rotation = Up},
            RotationType.Right => this with {Rotation = Right},
            RotationType.Down => this with {Rotation = Down},
            RotationType.Left => this with {Rotation = Left},
            _ => throw new NotSupportedException("Rotation is not supported.")
        };
    }

    public IEnumerable<int> GetIndizes()
    {
        yield return Rotation.I1;
        yield return Rotation.I2;
        yield return Rotation.I3;
        yield return Rotation.I4;
    }
}

public record LTetromino : Tetromino
{
    private LTetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new LTetromino(width).Rotate(rotation);
    protected override Rotation Up => new (1, Width + 1, Width * 2 + 1, 2);
    protected override Rotation Right => new (Width, Width + 1, Width + 2, Width * 2 + 2);
    protected override Rotation Down => new (1, Width + 1, Width * 2 + 1, Width * 2);
    protected override Rotation Left => new (Width, Width * 2, Width * 2 + 1, Width * 2 + 2);
}

public record ZTetromino : Tetromino
{
    private ZTetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new ZTetromino(width).Rotate(rotation);
    protected override Rotation Up => new (0,Width,Width+1,Width*2+1);
    protected override Rotation Right => new (Width+1, Width+2,Width*2,Width*2+1);
    protected override Rotation Down => new (Width+1, Width+2,Width*2,Width*2+1);
    protected override Rotation Left => new (Width+1, Width+2,Width*2,Width*2+1);
}

public record TTetromino : Tetromino
{
    private TTetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new TTetromino(width).Rotate(rotation);
    protected override Rotation Up => new (1,Width,Width+1,Width+2);
    protected override Rotation Right => new (1,Width+1,Width+2,Width*2+1);
    protected override Rotation Down => new (Width,Width+1,Width+2,Width*2+1);
    protected override Rotation Left => new (1,Width,Width+1,Width*2+1);
}

public record OTetromino : Tetromino
{
    private OTetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new OTetromino(width).Rotate(rotation);
    protected override Rotation Up => new (0,1,Width,Width+1);
    protected override Rotation Right => new (0,1,Width,Width+1);
    protected override Rotation Down => new (0,1,Width,Width+1);
    protected override Rotation Left => new (0,1,Width,Width+1);
}

public record ITetromino : Tetromino
{
    private ITetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new ITetromino(width).Rotate(rotation);
    protected override Rotation Up => new (1, Width + 1, Width * 2 + 1, Width * 3 + 1);
    protected override Rotation Right => new (Width,Width+1,Width+2,Width+32);
    protected override Rotation Down => new (1,Width+1,Width*2+1,Width*3+1);
    protected override Rotation Left => new (Width,Width+1,Width+2,Width+3);
}