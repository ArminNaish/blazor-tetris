namespace BlazorTetris.Domain;

public record LTetromino : Tetromino
{
    private LTetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new LTetromino(width).Rotate(rotation);
    protected override Rotation Up => new (1, Width + 1, Width * 2 + 1, 2);
    protected override Rotation Right => new (Width, Width + 1, Width + 2, Width * 2 + 2);
    protected override Rotation Down => new (1, Width + 1, Width * 2 + 1, Width * 2);
    protected override Rotation Left => new (Width, Width * 2, Width * 2 + 1, Width * 2 + 2);
}