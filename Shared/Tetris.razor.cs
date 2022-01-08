using BlazorTetris.Domain;
using BlazorTetris.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorTetris.Shared;

public partial class Tetris : IDisposable
{
    private Game Game { get; } = new();
    
    protected override void OnInitialized()
    {
        Game.OnStateChanged += StateChanged;
        Game.Initialize();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender) return;
        
        Game.Draw();
    }

    private void StateChanged()
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        Game.OnStateChanged -= StateChanged;
        Game.Dispose();
    }
}