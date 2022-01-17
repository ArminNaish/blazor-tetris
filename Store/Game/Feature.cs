using Fluxor;

namespace BlazorTetris.Store.Game;

public class GameFeature : Feature<GameState>
{
    public override string GetName() => "Game";

    protected override GameState GetInitialState() => new GameState();
}