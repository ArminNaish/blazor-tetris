using Microsoft.AspNetCore.Components;
using BlazorTetris.Store.Game;
using Fluxor;
using Microsoft.JSInterop;
using BlazorTetris.Services;

namespace BlazorTetris.Shared;

public partial class Tetris : IDisposable
{
    [Inject] private IState<GameState> State { get; set; } = null!;
    [Inject] private IDispatcher Dispatcher { get; set; } = null!;
    [Inject] private ITetrisJs TetrisJs { get; set; } = null!;

    private PeriodicTimer? _timer;

    protected async override Task OnInitializedAsync()
    {
        // Because we’ve inherited from a FluxorComponent, when we call OnInitialized(), our component subscribes to
        // state changes of the injected state type (GameState in our case), and when a new state is set
        // (from reducers producing a new state on an issued action), Blazor’s built in StateHasChanged()
        // is called for us, forcing components to re-render their markup accordingly.
        await base.OnInitializedAsync();

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

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            await TetrisJs.AddKeyUpEventListener(OnKeyUp);
        }
    }

    public void OnKeyUp(string key)
    {
        if (State.Value.Game is null) return;
        if (key == "ArrowLeft")
        {
            Dispatcher.Dispatch(new MoveTetrominoAction{Direction = Direction.Left});
        }
        // todo: add moveright
    } 

    protected override void Dispose(bool disposing)
    {
        TetrisJs.Dispose();
        _timer?.Dispose(); // Breaks the while loop and stops the timer
        
        base.Dispose(disposing);
    }
}
