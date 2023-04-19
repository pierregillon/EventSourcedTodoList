namespace TimeOnion.Configuration;

public abstract class TimedHostedService : IHostedService, IDisposable
{
    private readonly ManualResetEvent _autoResetEvent = new(true);
    private readonly ILogger<TimedHostedService> _logger;
    private Timer? _timer;
    protected TimedHostedService(ILogger<TimedHostedService> logger) => _logger = logger;

    protected abstract TimeSpan FromSeconds { get; }

    public void Dispose() => _timer?.Dispose();

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, FromSeconds, FromSeconds);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        _autoResetEvent.WaitOne();

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        _logger.LogInformation($"{GetType().Name} is triggered");

        _autoResetEvent.WaitOne();

        await DoWorkInternal();

        _autoResetEvent.Set();

        _logger.LogInformation($"{GetType().Name} has successfully ended");
    }

    protected abstract Task DoWorkInternal();
}