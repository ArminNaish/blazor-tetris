using BlazorTetris.Domain;

namespace BlazorTetris.Services;

public interface ITetrisJs 
{
    Task AddKeyUpEventListener(Action<string> action);
}