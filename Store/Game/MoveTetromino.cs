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
        
        return state with 
        { 
            Game = state.Game.Move(action.Direction)
        };
    }
}