namespace BlazorTetris.Domain;

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