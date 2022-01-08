using System.Collections.ObjectModel;
using Fluxor;
using BlazorTetris.Domain;

namespace BlazorTetris.Store.Game;

public record InitializeGameAction;

public record InitializeGameSuccessAction(
    List<Square> Squares, 
    List<Tetromino> Tetrominos, 
    Tetromino CurrentTetromino, 
    int CurrentPosition, 
    int CurrentRotation);

public record InitializeGameFailureAction(string ErrorMessage);

public class InitializeGameFeature : Feature<GameState>
{
    public override string GetName() => "Initialize";

    protected override GameState GetInitialState()
    {
        return new GameState(false, false, false, null, null, null, null, null, null);
    }
}

public static class InitializeGameActionsReducer
{
    [ReducerMethod]
    public static GameState ReduceInitializeGameAction(GameState state, InitializeGameAction _) =>
        state with
        {
            IsLoading = true,
            IsInitialized = false,
            HasErrors = false,
        };

    [ReducerMethod]
    public static GameState ReduceInitializeGameSuccessAction(GameState state, InitializeGameSuccessAction action) =>
        state with
        {
            IsLoading = false,
            IsInitialized = true,
            HasErrors = false,
            Squares = new ReadOnlyCollection<Square>(action.Squares),
            Tetrominos = new ReadOnlyCollection<Tetromino>(action.Tetrominos),
            CurrentTetromino = action.CurrentTetromino,
            CurrentPosition = action.CurrentPosition,
            CurrentRotation = action.CurrentRotation
        };

    [ReducerMethod]
    public static GameState ReduceInitializeGameFailureAction(GameState state, InitializeGameFailureAction action) =>
        state with
        {
            IsLoading = false,
            IsInitialized = false,
            HasErrors = true,
            ErrorMessage = action.ErrorMessage
        };
}

public class InitializeGameEffect : Effect<InitializeGameAction>
{
    private readonly ILogger<InitializeGameEffect> _logger;

    public InitializeGameEffect(ILogger<InitializeGameEffect> logger)
    {
        _logger = logger;
    }
    
    public override Task HandleAsync(InitializeGameAction action, IDispatcher dispatcher)
    {
        try
        {
            _logger.LogInformation("Initializing game...");

            var width = 10;
            var currentPosition = 4;
            var currentRotation = 0;
        
            var tetrominos = new List<Tetromino>
            {
                Tetromino.L(width), 
                Tetromino.Z(width), 
                Tetromino.T(width), 
                Tetromino.O(width), 
                Tetromino.I(width)
            };
        
            var squares = Enumerable.Range(0, 200)
                .Select(index => new Square{ Index = index})
                .ToList();

            var taken = Enumerable.Range(200, 10)
                .Select(index => new Square{ Index = index, IsTaken = true})
                .ToList();
        
            squares.AddRange(taken);
        
            var random = new Random();
            var currentTetromino = tetrominos[random.Next(0, tetrominos.Count)];
            
            _logger.LogInformation("Game initialized.");

            var initializeGameSuccessAction = new InitializeGameSuccessAction(
                squares,
                tetrominos,
                currentTetromino,
                currentPosition,
                currentRotation);
            
            dispatcher.Dispatch(initializeGameSuccessAction);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to initialize game: {ex.Message}");
            dispatcher.Dispatch(new InitializeGameFailureAction(ex.Message));
        }
        return Task.CompletedTask;
    }
}