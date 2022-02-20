using Microsoft.JSInterop;

namespace BlazorTetris.Services;

public class TetrisJs : ITetrisJs, IAsyncDisposable
{
    private readonly DotNetObjectReference<TetrisJs>? _reference;
    private readonly Task<IJSObjectReference> _importTask;
    private IJSObjectReference? _module;
    private Action<string>? _onKeyPress;

    public TetrisJs(IJSRuntime js)
    {
        _reference = DotNetObjectReference.Create(this);
        _importTask = js.InvokeAsync<IJSObjectReference>("import", "./js/tetris.js").AsTask();
    }

    public async Task AddKeyPressEventListener(Action<string> onKeyPress)
    {
        if (_module is null)
            _module = await _importTask;

        _onKeyPress = onKeyPress;
        await _module.InvokeVoidAsync("addKeyPressEventListener", _reference);
    }

    [JSInvokable]
    public void OnKeyPress(string key)
    {
        _onKeyPress?.Invoke(key);
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