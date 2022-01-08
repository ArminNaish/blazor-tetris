namespace BlazorTetris.Domain;

public record OTetromino : Tetromino
{
    private OTetromino(int width) => Width = width;
    public static Tetromino Of(int width, RotationType rotation = RotationType.Up) => new OTetromino(width).Rotate(rotation);
    protected override Rotation Up => new (0,1,Width,Width+1);
    protected override Rotation Right => new (0,1,Width,Width+1);
    protected override Rotation Down => new (0,1,Width,Width+1);
    protected override Rotation Left => new (0,1,Width,Width+1);
}