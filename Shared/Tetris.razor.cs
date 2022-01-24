using Microsoft.AspNetCore.Components;
using BlazorTetris.Store.Game;
using Fluxor;

namespace BlazorTetris.Shared;

public partial class Tetris : IDisposable
{
    [Inject] private IState<GameState> State { get; set; } = null!;
    [Inject] private IDispatcher Dispatcher { get; set; } = null!;

    private PeriodicTimer? _timer;

    protected async override Task OnInitializedAsync()
    {
        // Because we’ve inherited from a FluxorComponent, when we call OnInitialized(), our component subscribes to
        // state changes of the injected state type (GameState in our case), and when a new state is set
        // (from reducers producing a new state on an issued action), Blazor’s built in StateHasChanged()
        // is called for us, forcing components to re-render their markup accordingly.
        base.OnInitialized();

        if (State.Value.Game is null)
        {
            Dispatcher.Dispatch(new InitializeGameAction { });
        }

        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

        while (await _timer.WaitForNextTickAsync())
        {
            if (State.Value.Game is not null) 
            {
                Dispatcher.Dispatch(new RenderGameAction { });
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
         // Breaks the while loop and stops the timer
        _timer?.Dispose();
        
        base.Dispose(disposing);
    }
}