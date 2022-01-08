namespace BlazorTetris.Domain;

public record ZTetromino : Tetromino
{
    private ZTetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new ZTetromino(width).Rotate(rotation);
    protected override Rotation Up => new (0,Width,Width+1,Width*2+1);
    protected override Rotation Right => new (Width+1, Width+2,Width*2,Width*2+1);
    protected override Rotation Down => new (Width+1, Width+2,Width*2,Width*2+1);
    protected override Rotation Left => new (Width+1, Width+2,Width*2,Width*2+1);
}