using BlazorTetris.Domain;

namespace BlazorTetris.Services;

public interface ITetrisJs 
{
    Task AddKeyUpEventListener(Action<int> action);
}