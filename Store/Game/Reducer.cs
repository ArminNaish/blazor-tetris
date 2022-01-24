using BlazorTetris.Domain;

namespace BlazorTetris.Store.Game;

public static class Reducer
{
    private static readonly int _defaultPosition = 4;
    private static readonly int _width = 10;

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

    public static GameState.State SetDefaultPosition(this GameState.State state)
    {
        return state with { CurrentPosition = _defaultPosition };
    }

    public static GameState.State MoveToNextLine(this GameState.State state)
    {
        return state.SetCurrentPosition(state.CurrentPosition + _width);
    }

    public static GameState.State GetNewTetromino(this GameState.State state)
    {
        var tetrominos = new List<Tetromino>
        {
            LTetromino.Up(_width),
            ZTetromino.Up(_width),
            TTetromino.Up(_width),
            OTetromino.Up(_width),
            ITetromino.Up(_width),
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

    public static bool FrozenTetrominoAhead(this GameState.State state)
    {
        var currentIndizes = new[]{
            state.CurrentTetromino.I1 + state.CurrentPosition,
            state.CurrentTetromino.I2 + state.CurrentPosition,
            state.CurrentTetromino.I3 + state.CurrentPosition,
            state.CurrentTetromino.I4 + state.CurrentPosition
        };

        return state.Squares
            .Where(square => currentIndizes.Any(index => index + _width == square.Index))
            .Any(square => square.IsFrozen);
    }
}
