namespace BlazorTetris.Services;

public interface ITetrisJs 
{
    Task AddKeyPressEventListener(Action<string> action);
}