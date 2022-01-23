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
            currentTetromino.I1 + currentPosition, 
            currentTetromino.I2 + currentPosition, 
            currentTetromino.I3 + currentPosition, 
            currentTetromino.I4 + currentPosition
        }; 

        // undraw tetromino at current position
        squares = squares
            .Select(square => 
                currentIndizes.Contains(square.Index)
                    ? square with { IsTetromino = false }
                    : square)
            .ToList();

        currentPosition = currentPosition + action.Width;
        currentIndizes = new []{
            currentTetromino.I1 + currentPosition, 
            currentTetromino.I2 + currentPosition, 
            currentTetromino.I3 + currentPosition, 
            currentTetromino.I4 + currentPosition
        }; 

        // draw tetromino at next position
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