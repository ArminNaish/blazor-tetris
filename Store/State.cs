using BlazorTetris.Domain;

public abstract record RootState
(
    bool IsLoading, 
    string? ErrorMessage
);

public record GameState
(
    bool IsLoading, 
    bool IsInitialized,
    bool HasErrors,
    string? ErrorMessage, 
    IReadOnlyCollection<Square>? Squares, 
    IReadOnlyCollection<Tetromino>? Tetrominos, 
    Tetromino? CurrentTetromino, 
    int? CurrentPosition, 
    int? CurrentRotation
) : RootState(IsLoading, ErrorMessage);
