namespace BlazorTetris.Domain;

public static class Extensions
{
    public static Task LoopAsync(this Action action, int timeout, CancellationToken cancellationToken)
    {
        try
        {
            return Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(timeout, cancellationToken);
                    action();
                }
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Task cancelled
            return Task.CompletedTask;
        }
    }
}