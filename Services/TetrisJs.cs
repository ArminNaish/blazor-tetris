using BlazorTetris.Domain;
using Microsoft.JSInterop;

namespace BlazorTetris.Services;

public class TetrisJs : ITetrisJs, IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private IJSObjectReference? _module;

    public TetrisJs(IJSRuntime js)
    {
        _js = js;
    }

    public async Task ImportAsync()
    {
        _module = await _js.InvokeAsync<IJSObjectReference>("import", new object[]{"./js/tetris.js"});
    }

    // public async Task DrawAsync(Tetromino tetromino, int position)
    // {
    //     if (_module is not null)
    //     {
    //         var (i1, i2, i3, i4) = tetromino.Rotation;
    //         var indizes = new[] {i1, i2, i3, i4};
    //         await _module.InvokeVoidAsync("draw", indizes , position);
    //     }
    // }

    // public async Task UndrawAsync(Tetromino tetromino, int position)
    // {
    //     if (_module is not null)
    //     {
    //         var (i1, i2, i3, i4) = tetromino.Rotation;
    //         var indizes = new[] {i1, i2, i3, i4};
    //         await _module.InvokeVoidAsync("undraw", indizes, position);
    //     }
    // }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null) {
            await _module.DisposeAsync();
        }
    }
}
