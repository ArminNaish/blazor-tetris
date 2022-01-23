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
        var squares = state.Game!.Squares;/*.ToList();*/
        var currentTetromino = state.Game!.CurrentTetromino;
        var currentPosition = state.Game!.CurrentPosition;
        var currentIndizes = new []{
            currentTetromino.I1, 
            currentTetromino.I2, 
            currentTetromino.I3, 
            currentTetromino.I4
        }; 

        // undraw tetromino at current position
        squares = squares
            .Select(square => 
                currentIndizes.Contains(square.Index - currentPosition)
                    ? square with { IsTetromino = false }
                    : square)
            .ToList();

        currentPosition = currentPosition + action.Width;

        // draw tetromino at next position
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
                CurrentPosition = currentPosition
            }
        };
    }
}

public class RenderGameEffect : Effect<RenderGameAction>
{
    public override Task HandleAsync(RenderGameAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CheckCollisionAction{Width = action.Width});
        return Task.CompletedTask;
    }
}