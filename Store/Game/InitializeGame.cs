using Fluxor;

namespace BlazorTetris.Store.Game;

public record InitializeGameAction { }

public static class InitializeGameActionsReducer
{
    [ReducerMethod]
    public static GameState OnInitializeGame(GameState state, InitializeGameAction action)
    {
        return state with
        { 
            Game = Domain.Game.NewGame()
        };
    }
}
