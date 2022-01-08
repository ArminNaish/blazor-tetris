namespace BlazorTetris.Domain;

public record TTetromino : Tetromino
{
    private TTetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new TTetromino(width).Rotate(rotation);
    protected override Rotation Up => new (1,Width,Width+1,Width+2);
    protected override Rotation Right => new (1,Width+1,Width+2,Width*2+1);
    protected override Rotation Down => new (Width,Width+1,Width+2,Width*2+1);
    protected override Rotation Left => new (1,Width,Width+1,Width*2+1);
}