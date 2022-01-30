using BlazorTetris.Domain;

namespace BlazorTetris.Store.Game;

public static class Reducer
{
    public static GameState.State Squares(this GameState.State state)
    {
        var squares = Enumerable
            .Range(0, 210)
            .Select(index => new Square{ Index = index, IsFrozen = index >= 200})
            .ToList();

        return state with {Squares = squares};
    }

    public static GameState.State Initialize(this GameState.State state)
    {
        var position = 4;
        var width = 10;

        var tetrominos = new List<Tetromino>
        {
            LTetromino.Up(width),
            ZTetromino.Up(width),
            TTetromino.Up(width),
            OTetromino.Up(width),
            ITetromino.Up(width),
        };
        var random = new Random();
        var tetromino = tetrominos[random.Next(0, tetrominos.Count)];

        return state with 
        {
            CurrentTetromino = tetromino,
            CurrentPosition = position,
            Width = width
        };
    }


    public static GameState.State Draw(this GameState.State state)
    {
        var currentIndizes = new[]{
            state.CurrentTetromino.I1 + state.CurrentPosition,
            state.CurrentTetromino.I2 + state.CurrentPosition,
            state.CurrentTetromino.I3 + state.CurrentPosition,
            state.CurrentTetromino.I4 + state.CurrentPosition
        };

        var squares = state.Squares
            .Select(square =>
                currentIndizes.Contains(square.Index)
                    ? square with { IsTetromino = true }
                    : square)
            .ToList();

        return state with
        {
            Squares = squares,
        };
    }

    public static GameState.State Move(this GameState.State state, Direction direction)
    {
        
        return direction switch 
        {
            Direction.Left => state.MoveLeft(),
            _ => throw new NotSupportedException("Direction is not supported.")
        }; 
    }

    public static GameState.State MoveLeft(this GameState.State state)
    {
        var currentIndizes = new[]{
            state.CurrentTetromino.I1 + state.CurrentPosition,
            state.CurrentTetromino.I2 + state.CurrentPosition,
            state.CurrentTetromino.I3 + state.CurrentPosition,
            state.CurrentTetromino.I4 + state.CurrentPosition
        };

        // Check if tetromino touches the left border.
        // If so do not update the current position.
        if (currentIndizes.Any(index => index % 10 == 0))
            return state; 

        return state with { CurrentPosition = state.CurrentPosition - 1 };
    }

    public static GameState.State MoveRight(this GameState.State state)
    {
        var currentIndizes = new[]{
            state.CurrentTetromino.I1 + state.CurrentPosition,
            state.CurrentTetromino.I2 + state.CurrentPosition,
            state.CurrentTetromino.I3 + state.CurrentPosition,
            state.CurrentTetromino.I4 + state.CurrentPosition
        };

        // Check if tetromino touches the left border.
        // If so do not update the current position.
        if (currentIndizes.Any(index => index % 10 == 0))
            return state; 

        return state with { CurrentPosition = state.CurrentPosition + 1 };
    }

    public static GameState.State MoveDown(this GameState.State state)
    {
        return state with { CurrentPosition = state.CurrentPosition + state.Width };

    }

    public static GameState.State Undraw(this GameState.State state)
    { 
        var currentIndizes = new[]{
            state.CurrentTetromino.I1 + state.CurrentPosition,
            state.CurrentTetromino.I2 + state.CurrentPosition,
            state.CurrentTetromino.I3 + state.CurrentPosition,
            state.CurrentTetromino.I4 + state.CurrentPosition
        };

        var squares = state.Squares
            .Select(square =>
                currentIndizes.Contains(square.Index)
                    ? square with { IsTetromino = false }
                    : square)
            .ToList();

        return state with
        {
            Squares = squares,
        };
    }

    public static GameState.State Freeze(this GameState.State state)
    {
        var currentIndizes = new[]{
            state.CurrentTetromino.I1 + state.CurrentPosition,
            state.CurrentTetromino.I2 + state.CurrentPosition,
            state.CurrentTetromino.I3 + state.CurrentPosition,
            state.CurrentTetromino.I4 + state.CurrentPosition
        };

        var squares = state.Squares
            .Select(square =>
                currentIndizes.Contains(square.Index)
                    ? square with { IsFrozen = true }
                    : square)
            .ToList(); 
            
        return state with
        {
            Squares = squares,
        };  
    }
}
