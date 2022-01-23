using BlazorTetris.Domain;

namespace BlazorTetris.Services;

public interface ITetrisJs
{
     Task ImportAsync();
    // Task DrawAsync(Tetromino tetromino, int position);
    // Task UndrawAsync(Tetromino tetromino, int position);
}