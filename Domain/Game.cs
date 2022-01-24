namespace BlazorTetris.Domain;

public record Square
{
    public int Index { get; init; }
    public bool IsTetromino { get; init; }
    public bool IsFrozen { get; init; }
}

public record Tetromino(int I1, int I2, int I3, int I4) { }

public static class LTetromino
{
    public static Tetromino Up(int x) => new(1, x + 1, x * 2 + 1, 2);
    public static Tetromino Right(int x) => new(x, x + 1, x + 2, x * 2 + 2);
    public static Tetromino Down(int x) => new(1, x + 1, x * 2 + 1, x * 2);
    public static Tetromino Left(int x) => new(x, x * 2, x * 2 + 1, x * 2 + 2);
}

public static class ITetromino
{
    public static Tetromino Up(int x) => new(1, x + 1, x * 2 + 1, x * 3 + 1);
    public static Tetromino Right(int x) => new(x, x + 1, x + 2, x + 32);
    public static Tetromino Down(int x) => new(1, x + 1, x * 2 + 1, x * 3 + 1);
    public static Tetromino Left(int x) => new(x, x + 1, x + 2, x + 3);
}

public static class OTetromino
{
    public static Tetromino Up(int x) => new(0, 1, x, x + 1);
    public static Tetromino Right(int x) => new(0, 1, x, x + 1);
    public static Tetromino Down(int x) => new(0, 1, x, x + 1);
    public static Tetromino Left(int x) => new(0, 1, x, x + 1);
}

public static class TTetromino
{
    public static Tetromino Up(int x) => new(1, x, x + 1, x + 2);
    public static Tetromino Right(int x) => new(1, x + 1, x + 2, x * 2 + 1);
    public static Tetromino Down(int x) => new(x, x + 1, x + 2, x * 2 + 1);
    public static Tetromino Left(int x) => new(1, x, x + 1, x * 2 + 1);
}

public static class ZTetromino
{
    public static Tetromino Up(int x) => new(0, x, x + 1, x * 2 + 1);
    public static Tetromino Right(int x) => new(x + 1, x + 2, x * 2, x * 2 + 1);
    public static Tetromino Down(int x) => new(x + 1, x + 2, x * 2, x * 2 + 1);
    public static Tetromino Left(int x) => new(x + 1, x + 2, x * 2, x * 2 + 1);
}
