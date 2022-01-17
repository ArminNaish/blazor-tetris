namespace BlazorTetris.Store;

public record RootState
{
    public string? ErrorMessage { get; init; }
    public bool IsLoading { get; init; }
    public bool HasErrors => !string.IsNullOrEmpty(ErrorMessage);
}

