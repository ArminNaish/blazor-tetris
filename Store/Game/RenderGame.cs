using Fluxor;

namespace BlazorTetris.Store.Game;

public record RenderGameAction { }

public static class RenderGameActionsReducer
{
    [ReducerMethod]
    public static GameState OnRenderGame(GameState state, RenderGameAction action)
    {
        if (state.Game is null) throw new ArgumentNullException(nameof(state));

        var gameState = state.Game
            .Undraw()
            .MoveDown()
            .Draw();

        return state with { Game = gameState };
    }
}

public class RenderGameEffect : Effect<RenderGameAction>
{
    public override Task HandleAsync(RenderGameAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CheckCollisionAction { });
        return Task.CompletedTask;
    }
}