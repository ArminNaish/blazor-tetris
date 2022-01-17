namespace BlazorTetris.Domain;

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

    public bool HasIndex(int index)
    {
        return GetIndizes().Contains(index);
    }

    public IEnumerable<int> GetIndizes()
    {
        yield return Rotation.I1;
        yield return Rotation.I2;
        yield return Rotation.I3;
        yield return Rotation.I4;
    }
}