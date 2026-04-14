namespace AipsE2ETests.Infrastructure;

public static class WaitHelper
{
    public static async Task UntilAsync(Func<Task<bool>> condition, TimeSpan timeout, TimeSpan? interval = null)
    {
        interval ??= TimeSpan.FromMilliseconds(200);

        var start = DateTime.UtcNow;

        while (DateTime.UtcNow - start < timeout)
        {
            if (await condition())
            {
                return;
            }

            await Task.Delay(interval.Value);
        }

        throw new TimeoutException("Condition not met within timeout.");
    }
}