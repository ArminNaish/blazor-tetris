namespace BlazorTetris.Domain;

public record Game
{
    public int Width => 10;
    public int DefaultPosition => 4;
    public List<Square> Board { get; init; } = new();
    public Tetromino CurrentTetromino { get; init; } = null!;

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
        return this with
        {
            CurrentTetromino = Tetromino.Random(Width, DefaultPosition),
        };
    }

    public Game Draw()
    {
        var board = Board
            .Select(square => CurrentTetromino.Contains(square.Index)
                ? square with { IsTetromino = true }
                : square)
            .ToList();

        return this with { Board = board };
    }

    public Game Undraw()
    {
        var board = Board
            .Select(square => CurrentTetromino.Contains(square.Index)
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
        var collision = Board
            .Where(square => CurrentTetromino.Indizes().Any(index => index + Width == square.Index))
            .Any(square => square.IsFrozen);

        if (!collision)
            return this;

        return FreezeTetromino()
            .RandomTetromino()
            .Draw();
    }

    public Game FreezeTetromino()
    {
        var board = Board
            .Select(square =>
                CurrentTetromino.Contains(square.Index)
                    ? square with { IsFrozen = true }
                    : square)
            .ToList();

        return this with { Board = board };
    }

    private Game MoveLeft()
    {
        var collision = Board
            .Where(square => CurrentTetromino.Indizes().Any(index => index - 1 == square.Index))
            .Any(square => square.IsFrozen);

        if (collision)
            return this;

        return this with
        {
            CurrentTetromino = CurrentTetromino.MoveLeft()
        };
    }

    private Game MoveRight()
    {
        var collision = Board
            .Where(square => CurrentTetromino.Indizes().Any(index => index + 1 == square.Index))
            .Any(square => square.IsFrozen);

        if (collision)
            return this;

        return this with
        {
            CurrentTetromino = CurrentTetromino.MoveRight()
        };
    }

    private Game MoveDown()
    {
        return this with
        {
            CurrentTetromino = CurrentTetromino.MoveDown(Width)
        };
    }

    private Game Rotate()
    {
        return this with
        {
            CurrentTetromino = CurrentTetromino.Rotate()
        };
    }
}

public record Square
{
    public int Index { get; init; }
    public bool IsTetromino { get; init; }
    public bool IsFrozen { get; init; }
}

public record Rotation
{
    public RotationDirection Direction { get; init; }
    public int I1 { get; init; }
    public int I2 { get; init; }
    public int I3 { get; init; }
    public int I4 { get; init; }
    public static implicit operator int(Rotation rotation) => (int)rotation.Direction;
}

public record Tetromino
{
    public List<Rotation> Rotations { get; init; } = new();
    public Rotation Rotation { get; init; } = null!;
    public int Position { get; init; }
    public int Width { get; init; }

    public static Tetromino Random(int width, int position)
    {
        var random = new Random();

        var rotations = random.Next(0, 5) switch
        {
            (int)TetrominoType.L => Tetrominoes.L(width),
            (int)TetrominoType.I => Tetrominoes.I(width),
            (int)TetrominoType.O => Tetrominoes.O(width),
            (int)TetrominoType.T => Tetrominoes.T(width),
            (int)TetrominoType.Z => Tetrominoes.Z(width),
            _ => throw new NotSupportedException($"Tetromino type is not supported.")
        };

        // todo: what is the default rotation? Up or random?
        return new Tetromino
        {
            Rotations = rotations,
            Rotation = rotations[(int)RotationDirection.Up],
            Position = position,
            Width = width
        };
    }

    public Tetromino Rotate()
    {
        var current = (int)Rotation;
        var next = (current + 1) <= 3 ? (current + 1) : 0;
        return this with
        {
            Rotation = Rotations[next]
        };
    }

    public Tetromino MoveLeft()
    {
        // Check if tetromino touches the left border,
        // if so do not update the current position.
        if (Indizes().Any(index => index % 10 == 0))
            return this;

        return this with { Position = Position - 1 };
    }

    public Tetromino MoveRight()
    {
        var nextPosition = Position + 1;
        var nextIndizes = Indizes(nextPosition);

        // Check if tetromino touches the left border,
        // if so do not update the current position.
        if (nextIndizes.Any(index => index % 10 == 0))
            return this;

        return this with { Position = nextPosition };
    }

    public Tetromino MoveDown(int width)
    {
        return this with { Position = Position + width };
    }

    public bool Contains(int index)
    {
        return Rotation.I1 + Position == index ||
            Rotation.I2 + Position == index ||
            Rotation.I3 + Position == index ||
            Rotation.I4 + Position == index;
    }

    public IEnumerable<int> Indizes()
    {
        return Indizes(Position);
    }

    private IEnumerable<int> Indizes(int position)
    {
        yield return Rotation.I1 + position;
        yield return Rotation.I2 + position;
        yield return Rotation.I3 + position;
        yield return Rotation.I4 + position;
    }
}

public static class Tetrominoes
{
    public static List<Rotation> L(int width)
    {
        return new List<Rotation>
        {
            new Rotation{Direction = RotationDirection.Up,   I1 = 1, I2 =width + 1, I3 = width * 2 + 1, I4 = 2},
            new Rotation{Direction = RotationDirection.Right,I1 = width,I2 = width + 1, I3 =width + 2, I4 =width * 2 + 2},
            new Rotation{Direction = RotationDirection.Down, I1 = 1, I2 =width + 1,I3 = width * 2 + 1,I4 = width * 2},
            new Rotation{Direction = RotationDirection.Left, I1 = width,I2 = width * 2,I3 = width * 2 + 1,I4 = width * 2 + 2}
        };
    }

    public static List<Rotation> I(int width)
    {
        return new List<Rotation>
        {
            new Rotation{Direction = RotationDirection.Up,   I1 = 1, I2=width + 1,I3= width * 2 + 1,I4=width * 3 + 1},
            new Rotation{Direction = RotationDirection.Right,I1 = width,I2= width + 1,I3= width + 2,I4=width + 3},
            new Rotation{Direction = RotationDirection.Down, I1 = 1,I2= width + 1,I3= width * 2 + 1,I4=width * 3 + 1},
            new Rotation{Direction = RotationDirection.Left, I1 = width,I2= width + 1, I3=width + 2,I4=width + 3}
        };
    }

    public static List<Rotation> O(int width)
    {
        return new List<Rotation>
        {
            new Rotation{Direction = RotationDirection.Up,   I1 =0, I2 = 1,I3=width,I4 = width + 1},
            new Rotation{Direction = RotationDirection.Right,I1 =0, I2 = 1,I3=width,I4 = width + 1},
            new Rotation{Direction = RotationDirection.Down, I1 =0, I2 = 1,I3=width,I4 = width + 1},
            new Rotation{Direction = RotationDirection.Left, I1 =0, I2 = 1,I3=width,I4 = width + 1},
        };
    }

    public static List<Rotation> T(int width)
    {
        return new List<Rotation>
        {
            new Rotation{Direction = RotationDirection.Up,   I1=1,  I2 =width, I3=width + 1,I4 =  width + 2},
            new Rotation{Direction = RotationDirection.Right,I1=1,  I2 =width + 1,I3= width + 2,I4 =  width * 2 + 1},
            new Rotation{Direction = RotationDirection.Down, I1=width,  I2 =width + 1,I3= width + 2, I4 = width * 2 + 1},
            new Rotation{Direction = RotationDirection.Left, I1=1,  I2 =width, I3=width + 1,I4 =  width * 2 + 1},
        };
    }

    public static List<Rotation> Z(int width)
    {
        return new List<Rotation>
        {
            new Rotation{Direction = RotationDirection.Up,   I1=0,  I2 =width, I3=width + 1,I4 =  width * 2 + 1},
            new Rotation{Direction = RotationDirection.Right,I1=width + 1, I2 = width + 2, I3=width * 2, I4 = width * 2 + 1},
            new Rotation{Direction = RotationDirection.Down, I1=0, I2 = width, I3=width +1, I4 = width * 2 + 1},
            new Rotation{Direction = RotationDirection.Left, I1=width + 1, I2 = width + 2, I3=width * 2, I4 = width * 2 + 1},
        };
    }
}

public enum Direction
{
    Left = 37,
    Up = 38,
    Right = 39,
    Down = 40
}

public enum TetrominoType
{
    L = 0,
    Z = 1,
    T = 2,
    O = 3,
    I = 4,
}

public enum RotationDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}
