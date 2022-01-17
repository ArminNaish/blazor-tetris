using Fluxor;

namespace BlazorTetris.Store.Game;

public record RenderGameAction
{
    public int Width { get; init; }
}

public static class RenderGameActionsReducer
{
    [ReducerMethod]
    public static GameState ReduceRenderGameAction(GameState state, RenderGameAction action)
    {
        var squares = state.Game!.Squares.ToList();
        var currentTetromino = state.Game!.CurrentTetromino;
        var currentPosition = state.Game!.CurrentPosition;

        // undraw tetromino at current position
        squares = squares
            .Select(square => currentTetromino.HasIndex(square.Index - currentPosition)
                ? square with { IsTetromino = false }
                : square)
            .ToList();

        // update position
        var nextPosition = currentPosition + action.Width;

        // draw tetromino at next position
        squares = squares
            .Select(square => currentTetromino.HasIndex(square.Index - nextPosition)
                ? square with { IsTetromino = true }
                : square)
            .ToList();
      
        return state with
        {
            Game = state.Game with
            {
                CurrentPosition = nextPosition,
                Squares = squares
            }
        };
    }
}