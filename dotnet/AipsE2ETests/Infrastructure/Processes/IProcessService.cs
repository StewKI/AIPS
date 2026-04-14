namespace AipsE2ETests.Infrastructure.Processes;

public interface IProcessService : IAsyncDisposable
{
    Task StartProcessAsync();
}