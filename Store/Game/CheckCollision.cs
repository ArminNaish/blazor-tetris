using Fluxor;

namespace BlazorTetris.Store.Game;

public record CheckCollisionAction { }

public static class CheckCollisionActionReducer
{
    [ReducerMethod]
    public static GameState OnCheckCollision(GameState state, CheckCollisionAction action)
    {
        if (state.Game is null) throw new ArgumentNullException(nameof(state));

        var currentIndizes = new[]{
            state.Game.CurrentTetromino.I1 + state.Game.CurrentPosition,
            state.Game.CurrentTetromino.I2 + state.Game.CurrentPosition,
            state.Game.CurrentTetromino.I3 + state.Game.CurrentPosition,
            state.Game.CurrentTetromino.I4 + state.Game.CurrentPosition
        };

        var frozenTetrominoesAhead = state.Game.Squares
            .Where(square => currentIndizes.Any(index => index + state.Game.Width == square.Index))
            .Any(square => square.IsFrozen);

        if (!frozenTetrominoesAhead)
            return state;

        var gameState = state.Game
            .Freeze()
            .Initialize()
            .Draw();

        return state with { Game = gameState };
    }
}
