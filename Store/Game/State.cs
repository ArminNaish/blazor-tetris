namespace BlazorTetris.Store.Game;

using BlazorTetris.Domain;

public record GameState : RootState
{
    public Game? Game { get; init; }
}

