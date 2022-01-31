using Fluxor;

namespace BlazorTetris.Store.Game;

public enum Direction : int
{
    Left = 37,
    Up = 38,
    Right = 39,
    Down = 40
}

public record MoveTetrominoAction{ 
    public Direction Direction{get; init;}
}

public static class MoveTetrominoActionsReducer
{
    [ReducerMethod]
    public static GameState OnMoveTetromino(GameState state, MoveTetrominoAction action)
    {
        if (state.Game is null) throw new ArgumentNullException(nameof(state));
        
        var gameState  = state.Game
            .Undraw()
            .Move(action.Direction)
            .Draw();

        return state with { Game = gameState };
    }
}