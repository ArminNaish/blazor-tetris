using BlazorTetris.Domain;
using Fluxor;

namespace BlazorTetris.Store.Game;

public record CheckCollisionAction
{
    public int Width { get; init; }
}

// TODO: how to share code between reducers? --> use 
//https://redux.js.org/usage/structuring-reducers/reusing-reducer-logic

public static class CheckCollisionActionReducer
{
    [ReducerMethod]
    public static GameState ReduceCheckCollisionAction(GameState state, CheckCollisionAction action)
    {
        var squares = state.Game!.Squares;//.ToList();
        var currentTetromino = state.Game!.CurrentTetromino;
        var currentPosition = state.Game!.CurrentPosition;
        var currentIndizes = new []{
            currentTetromino.I1 + currentPosition, 
            currentTetromino.I2 + currentPosition, 
            currentTetromino.I3 + currentPosition, 
            currentTetromino.I4 + currentPosition
        }; 

        var takenSquaresAhead = squares
            .Where(square => currentIndizes.Any(i => i + action.Width == square.Index ))
            .Any(square => square.IsTaken);

        if (!takenSquaresAhead)
            return state;

        squares = squares
            .Select(square =>
                currentIndizes.Contains(square.Index)
                    ? square with { IsTaken = true }
                    : square)
            .ToList();

        // Draw a new tetromino at the top
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

        currentPosition = 4;

        currentIndizes = new []{
            currentTetromino.I1 + currentPosition, 
            currentTetromino.I2 + currentPosition, 
            currentTetromino.I3 + currentPosition, 
            currentTetromino.I4 + currentPosition
        }; 

        squares = squares
            .Select(square =>  
                currentIndizes.Contains(square.Index)
                    ? square with { IsTetromino = true }
                    : square)
            .ToList();

        return state with
        {
            Game = state.Game with
            {
                Squares = squares,
                CurrentTetromino = currentTetromino,
                CurrentPosition = currentPosition
            }
        };
    }
}
