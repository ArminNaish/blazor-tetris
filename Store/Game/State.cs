namespace BlazorTetris.Store.Game;

using BlazorTetris.Domain;

public record GameState : RootState
{
    public State? Game { get; init; }

    public record State
    {
        public IReadOnlyCollection<Square> Squares { get; init; } = null!;
        public Tetromino CurrentTetromino { get; init; } = null!;
        public int CurrentPosition { get; init; }
        public int CurrentRotation { get; init; }
    }
}

