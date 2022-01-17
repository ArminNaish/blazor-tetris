using System.Collections.ObjectModel;
using Fluxor;
using BlazorTetris.Domain;

namespace BlazorTetris.Store.Game;

public record InitializeGameAction
{
    public int Width { get; init; }
}

public static class InitializeGameActionsReducer
{
    [ReducerMethod]
    public static GameState ReduceInitializeGameAction(GameState state, InitializeGameAction action)
    {
        var defaultPosition = 4;
        var defaultRotation = 0;

        var tetrominos = new List<Tetromino>
        {
            Tetromino.L(action.Width),
            Tetromino.Z(action.Width),
            Tetromino.T(action.Width),
            Tetromino.O(action.Width),
            Tetromino.I(action.Width)
        };

        var random = new Random();
        var currentTetromino = tetrominos[random.Next(0, tetrominos.Count)];

        var squares = Enumerable
            .Range(0, 200)
            .Select(index => new Square 
            {
                Index = index, 
                IsTetromino = currentTetromino.HasIndex(index - defaultPosition)
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
                Tetrominos = new ReadOnlyCollection<Tetromino>(tetrominos),
                CurrentTetromino = currentTetromino,
                CurrentPosition = defaultPosition,
                CurrentRotation = defaultRotation
            }
        };
    }
}
