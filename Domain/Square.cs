namespace BlazorTetris.Domain;

public record Square
{
    public int Index { get; init; }
    public bool IsTetromino { get; init; }
    public bool IsTaken { get; init; }
}