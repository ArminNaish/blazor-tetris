using Fluxor;

namespace BlazorTetris.Store.Game;

public record CheckCollisionAction { }

public static class CheckCollisionActionReducer
{
    [ReducerMethod]
    public static GameState OnCheckCollision(GameState state, CheckCollisionAction action)
    {
        if (state.Game is null) throw new ArgumentNullException(nameof(state));

        if (!state.Game.FrozenTetrominoAhead())
            return state;

        var gameState = state.Game!
            .FreezeCurrentTetromino()
            .GetNewTetromino()
            .SetDefaultPosition()
            .DrawCurrentTetromino();

        return state with { Game = gameState };
    }
}
