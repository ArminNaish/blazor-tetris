using Microsoft.JSInterop;

namespace BlazorTetris.Services;

public class TetrisJs : ITetrisJs
{
    private readonly IJSRuntime _js;
    private Action<string>? _onKeyUp;
    private DotNetObjectReference<TetrisJs>? _reference;

    public TetrisJs(IJSRuntime js)
    {
        _js = js;
    }

    public async Task AddKeyUpEventListener(Action<string> onKeyUp)
    {
        if (_reference is not null) return;

        _onKeyUp = onKeyUp;
        _reference = DotNetObjectReference.Create(this);
        await _js.InvokeVoidAsync("addKeyUpEventListener", _reference);
    }

    [JSInvokable]
    public void OnKeyUp(string key)
    {
        _onKeyUp?.Invoke(key);
    } 

    public void Dispose()
    {
        _reference?.Dispose();
    }
}