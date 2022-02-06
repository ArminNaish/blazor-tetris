using Fluxor;
using BlazorTetris.Domain;

namespace BlazorTetris.Store.Game;

public record InitializeGameAction { }

public static class InitializeGameActionsReducer
{
    [ReducerMethod]
    public static GameState OnInitializeGame(GameState state, InitializeGameAction action)
    {
        var game = new Domain.Game()
            .Initialize()
            .RandomTetromino()
            .Draw();

        return state with { Game = game };
    }
}
