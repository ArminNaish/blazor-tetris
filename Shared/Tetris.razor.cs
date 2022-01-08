using BlazorTetris.Domain;
using BlazorTetris.Services;
using Microsoft.AspNetCore.Components;
using BlazorTetris.Store;
using BlazorTetris.Services;
using BlazorTetris.Store.Game;
using Fluxor;

namespace BlazorTetris.Shared;

public partial class Tetris //: IDisposable
{
    [Inject] private IState<GameState> State { get; set; }
    [Inject] private IDispatcher Dispatcher { get; set; }

    // todo: replace Game with fluxor states
    // todo: add initialize feature
    // todo: add draw feature
    // todo: add movedown feature
    
    //private Game Game { get; } = new();
    
    protected override void OnInitialized()
    {
        if (State.Value.Squares is null)
        {
            Dispatcher.Dispatch(new InitializeGameAction());
        }
        
        // Because we’ve inherited from a FluxorComponent, when we call OnInitialized(), our component subscribes to
        // state changes of the injected state type (GameState in our case), and when a new state is set
        // (from reducers producing a new state on an issued action), Blazor’s built in StateHasChanged()
        // is called for us, forcing components to re-render their markup accordingly.
        base.OnInitialized();
        
        
        //Game.OnStateChanged += StateChanged;
        //Game.Initialize();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender) return;
        
        //Game.Draw();
        // todo: call base
    }

    //private void StateChanged()
    //{
      //  StateHasChanged();
    //}

    //public void Dispose()
    //{
        //Game.OnStateChanged -= StateChanged;
        //Game.Dispose();
    //}
}