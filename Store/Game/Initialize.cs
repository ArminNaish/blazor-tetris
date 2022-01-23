using System.Collections.ObjectModel;
using Fluxor;
using BlazorTetris.Domain;

namespace BlazorTetris.Store.Game;

public record InitializeGameAction
{
    public int Width { get; init; }
    public int Position { get; init; }
}

public static class InitializeGameActionsReducer
{
    [ReducerMethod]
    public static GameState ReduceInitializeGameAction(GameState state, InitializeGameAction action)
    {
        var random = new Random();
        var currentPosition = 4;

        var tetrominos = new List<Tetromino>
        {
            LTetromino.Up(action.Width),
            ZTetromino.Up(action.Width),
            TTetromino.Up(action.Width),
            OTetromino.Up(action.Width),
            ITetromino.Up(action.Width),
        };
        
        var currentTetromino = tetrominos[random.Next(0, tetrominos.Count)];
        var currentIndizes = new []{
            currentTetromino.I1, 
            currentTetromino.I2, 
            currentTetromino.I3, 
            currentTetromino.I4
        }; 

        var squares = Enumerable
            .Range(0, 200)
            .Select(index => new Square
            {
                Index = index,
                IsTetromino = currentIndizes.Contains(index - currentPosition)
            })
            .ToList();

        var taken = Enumerable.Range(200, 10)
            .Select(index => new Square { Index = index, IsTaken = true })
            .ToList();

        squares.AddRange(taken);

        return state with
        {
            Game = new GameState.State
            {
                Squares = new ReadOnlyCollection<Square>(squares),
                CurrentTetromino = currentTetromino,
                CurrentPosition = currentPosition
            }
        };
    }
}
