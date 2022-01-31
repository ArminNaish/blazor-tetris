using Microsoft.JSInterop;

namespace BlazorTetris.Services;

public class TetrisJs : ITetrisJs, IAsyncDisposable
{
    private readonly DotNetObjectReference<TetrisJs>? _reference;
    private readonly Task<IJSObjectReference> _importTask;
    private IJSObjectReference? _module;
    private Action<int>? _onKeyUp;

    public TetrisJs(IJSRuntime js)
    {
        _reference = DotNetObjectReference.Create(this);
        _importTask = js.InvokeAsync<IJSObjectReference>("import", "./js/tetris.js").AsTask();
    }

    public async Task AddKeyUpEventListener(Action<int> onKeyUp)
    {
        if (_module is null)
            _module = await _importTask;

        _onKeyUp = onKeyUp;
        await _module.InvokeVoidAsync("addKeyUpEventListener", _reference);
    }

    [JSInvokable]
    public void OnKeyUp(int key)
    {
        _onKeyUp?.Invoke(key);
    } 

    public async ValueTask DisposeAsync()
    {
        _reference?.Dispose();
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}