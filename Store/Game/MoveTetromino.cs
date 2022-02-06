using BlazorTetris.Domain;
using Fluxor;

namespace BlazorTetris.Store.Game;

public record MoveTetrominoAction{ 
    public Direction Direction{get; init;}
}

public static class MoveTetrominoActionsReducer
{
    [ReducerMethod]
    public static GameState OnMoveTetromino(GameState state, MoveTetrominoAction action)
    {
        if (state.Game is null) throw new ArgumentNullException(nameof(state));
        
        var game = state.Game
            .Undraw()
            .Move(action.Direction)
            .Draw()
            .CheckCollision();

        return state with { Game = game };
    }
}