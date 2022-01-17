using System.Collections.ObjectModel;
using Fluxor;
using BlazorTetris.Domain;

namespace BlazorTetris.Store.Game;

/*
public record UpdateGameAction;

public record UpdateGameSuccessAction;

public record UpdateGameFailureAction(string ErrorMessage);

public class UpdateGameFeature : Feature<GameState>
{
    public override string GetName() => "Update Game";

    protected override GameState GetInitialState() => new GameState();
}

public static class UpdateGameActionsReducer
{
    [ReducerMethod]
    public static GameState ReduceUpdateGameAction(GameState state, InitializeGameAction _) =>
        state with
        {
            IsLoading = false
        };

    [ReducerMethod]
    public static GameState ReduceUpdateGameSuccessAction(GameState state, InitializeGameSuccessAction action) =>
        state with
        {
            IsLoading = false,
            Game = state.Game! with
            {
                Squares = action.Squares,
                Tetrominos = action.Tetrominos,
                CurrentTetromino = action.CurrentTetromino,
                CurrentPosition = action.CurrentPosition,
                CurrentRotation = action.CurrentRotation
            }
        };

    [ReducerMethod]
    public static GameState ReduceUpdateGameFailureAction(GameState state, InitializeGameFailureAction action) =>
        state with
        {
            IsLoading = false,
            ErrorMessage = action.ErrorMessage
        };
}
*/