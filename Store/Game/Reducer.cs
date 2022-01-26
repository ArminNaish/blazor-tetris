using BlazorTetris.Domain;

namespace BlazorTetris.Store.Game;

public static class Reducer
{
    public static GameState.State GetSquares(this GameState.State state)
    {
        var squares = Enumerable
            .Range(0, 210)
            .Select(index => new Square{ Index = index, IsFrozen = index >= 200})
            .ToList();

        return state with {Squares = squares};
    }

    public static GameState.State SetCurrentPosition(this GameState.State state, int position)
    {
        return state with { CurrentPosition = position };
    }

    public static GameState.State Initialize(this GameState.State state)
    {
        return state with 
        {
             CurrentPosition = 4,
             Width = 10
        };
    }

    public static GameState.State MoveToNextLine(this GameState.State state)
    {
        return state.SetCurrentPosition(state.CurrentPosition + state.Width);
    }

    public static GameState.State GetNewTetromino(this GameState.State state)
    {
        var tetrominos = new List<Tetromino>
        {
            LTetromino.Up(state.Width),
            ZTetromino.Up(state.Width),
            TTetromino.Up(state.Width),
            OTetromino.Up(state.Width),
            ITetromino.Up(state.Width),
        };
        var random = new Random();
        var currentTetromino = tetrominos[random.Next(0, tetrominos.Count)];
        return state with { CurrentTetromino = currentTetromino };
    }

    public static GameState.State DrawCurrentTetromino(this GameState.State state)
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

    public static GameState.State UndrawCurrentTetromino(this GameState.State state)
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

    public static GameState.State FreezeCurrentTetromino(this GameState.State state)
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
