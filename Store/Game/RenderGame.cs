using Fluxor;
using BlazorTetris.Domain;

namespace BlazorTetris.Store.Game;

public record RenderGameAction { }

public static class RenderGameActionsReducer
{
    [ReducerMethod]
    public static GameState OnRenderGame(GameState state, RenderGameAction action)
    {
        if (state.Game is null) throw new ArgumentNullException(nameof(state));

        var game = state.Game
            .Undraw()
            .Move(Direction.Down)
            .Draw()
            .CheckCollision();
  
        return state with { Game = game };
    }
}

