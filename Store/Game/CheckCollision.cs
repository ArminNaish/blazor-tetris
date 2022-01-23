using BlazorTetris.Domain;
using Fluxor;

namespace BlazorTetris.Store.Game;

public record CheckCollisionAction
{
    public int Width { get; init; }
}

public static class CheckCollisionActionReducer
{
    [ReducerMethod]
    public static GameState ReduceCheckCollisionAction(GameState state, CheckCollisionAction action)
    {
        var squares = state.Game!.Squares;//.ToList();
        var currentTetromino = state.Game!.CurrentTetromino;
        var currentPosition = state.Game!.CurrentPosition;
        var currentIndizes = new []{
            currentTetromino.I1, 
            currentTetromino.I2, 
            currentTetromino.I3, 
            currentTetromino.I4
        }; 

        // todo: fix this broken feature
        var takenSquaresAhead = squares
            .Where(square => currentIndizes.Contains(square.Index - currentPosition + action.Width))
            .Any(square => square.IsTaken);

        if (!takenSquaresAhead)
            return state;

        squares = squares
            .Select(square =>
                currentIndizes.Contains(square.Index - currentPosition)
                    ? square with { IsTaken = true }
                    : square)
            .ToList();


        var tetrominos = new List<Tetromino>
        {
            LTetromino.Up(action.Width),
            ZTetromino.Up(action.Width),
            TTetromino.Up(action.Width),
            OTetromino.Up(action.Width),
            ITetromino.Up(action.Width),
        };
        var random = new Random();
        currentTetromino = tetrominos[random.Next(0, tetrominos.Count)];

        squares = squares
            .Select(square =>  
                currentIndizes.Contains(square.Index - currentPosition)
                    ? square with { IsTetromino = true }
                    : square)
            .ToList();

        return state with
        {
            Game = state.Game with
            {
                Squares = squares,
                CurrentTetromino = currentTetromino,
            }
        };
    }
}
