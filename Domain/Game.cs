namespace BlazorTetris.Domain;

public enum Direction : int
{
    Left = 37,
    Up = 38,
    Right = 39,
    Down = 40
}

public record Game
{
    public int Width => 10;
    public int DefaultPosition => 4;
    public List<Square> Board { get; init; } = new();
    public Tetromino CurrentTetromino { get; init; } = null!;
    public int CurrentPosition { get; init; }

    public Game Initialize()
    {
        var board = Enumerable
            .Range(0, 210)
            .Select(index => new Square { Index = index, IsFrozen = index >= 200 })
            .ToList();

        return this with
        {
            Board = board
        };
    }

    public Game RandomTetromino()
    {
        var tetrominos = new List<Tetromino>
        {
            LTetromino.Up(Width),
            ZTetromino.Up(Width),
            TTetromino.Up(Width),
            OTetromino.Up(Width),
            ITetromino.Up(Width),
        };
        var random = new Random();
        var tetromino = tetrominos[random.Next(0, tetrominos.Count)];

        return this with
        {
            CurrentPosition = DefaultPosition,
            CurrentTetromino = tetromino,
        };
    }

    public Game Draw()
    {
        var indizes = CurrentTetromino.Indizes(at: CurrentPosition);
        var board = Board
            .Select(square =>
                indizes.Contains(square.Index)
                    ? square with { IsTetromino = true }
                    : square)
            .ToList();

        return this with { Board = board };
    }

    public Game Undraw()
    {
        var indizes = CurrentTetromino.Indizes(at: CurrentPosition);
        var board = Board
            .Select(square =>
                indizes.Contains(square.Index)
                    ? square with { IsTetromino = false }
                    : square)
            .ToList();
        
        return this with { Board = board };
    }

    public Game Move(Direction direction)
    {
        return direction switch
        {
            Direction.Left => MoveLeft(),
            Direction.Up => Rotate(),
            Direction.Right => MoveRight(),
            Direction.Down => MoveDown(),
            _ => throw new NotSupportedException("Direction is not supported.")
        };
    }

    public Game CheckCollision()
    {
        var indizes = CurrentTetromino.Indizes(at: CurrentPosition);
        var collision = Board
            .Where(square => indizes.Any(index => index + Width == square.Index))
            .Any(square => square.IsFrozen);

        if (!collision)
            return this;

        return FreezeTetromino()
            .RandomTetromino()
            .Draw();
    }

    public Game FreezeTetromino()
    {
        var indizes = CurrentTetromino.Indizes(at: CurrentPosition);

        var board = Board
            .Select(square =>
                indizes.Contains(square.Index)
                    ? square with { IsFrozen = true }
                    : square)
            .ToList();

        return this with { Board = board };
    }

    private Game MoveLeft()
    {
        var indizes = CurrentTetromino.Indizes(at: CurrentPosition);

        // Check if tetromino touches the left border,
        // if so do not update the current position.
        if (indizes.Any(index => index % 10 == 0))
            return this;

        return this with { CurrentPosition = CurrentPosition - 1 };
    }

    private Game MoveRight()
    {
        var nextPosition = CurrentPosition + 1;
        var nextIndizes = CurrentTetromino.Indizes(at: nextPosition);

        // Check if tetromino touches the left border,
        // if so do not update the current position.
        if (nextIndizes.Any(index => index % 10 == 0))
            return this;

        return this with { CurrentPosition = nextPosition };
    }

    private Game MoveDown()
    {
        return this with { CurrentPosition = CurrentPosition + Width };
    }

    private Game Rotate()
    {
        throw new NotImplementedException();
    }
}

public record Square
{
    public int Index { get; init; }
    public bool IsTetromino { get; init; }
    public bool IsFrozen { get; init; }
}

public record Tetromino(int I1, int I2, int I3, int I4)
{

    public int[] Indizes(int at)
    {
        return new[] {
            I1 + at,
            I2 + at,
            I3 + at,
            I4 + at
        };
    }

}

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
