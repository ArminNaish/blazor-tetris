namespace BlazorTetris.Domain;

public record ITetromino : Tetromino
{
    private ITetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new ITetromino(width).Rotate(rotation);
    protected override Rotation Up => new (1, Width + 1, Width * 2 + 1, Width * 3 + 1);
    protected override Rotation Right => new (Width,Width+1,Width+2,Width+32);
    protected override Rotation Down => new (1,Width+1,Width*2+1,Width*3+1);
    protected override Rotation Left => new (Width,Width+1,Width+2,Width+3);
}