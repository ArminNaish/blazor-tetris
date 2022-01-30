using BlazorTetris.Domain;

namespace BlazorTetris.Services;

public interface ITetrisJs : IDisposable
{
    Task AddKeyUpEventListener(Action<string> action);
}