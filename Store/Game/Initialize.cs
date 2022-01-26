using Fluxor;

namespace BlazorTetris.Store.Game;

public record InitializeGameAction { }

public static class InitializeGameActionsReducer
{
    [ReducerMethod]
    public static GameState OnInitializeGame(GameState state, InitializeGameAction action)
    {
        var gameState = new GameState.State()
            .Initialize()
            .GetNewTetromino()
            .GetSquares()
            .DrawCurrentTetromino();

        return state with { Game = gameState };
    }
}
