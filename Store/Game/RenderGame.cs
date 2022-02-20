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

        return state with 
        { 
            Game = state.Game.Move(Direction.Down)
        };
    }
}

