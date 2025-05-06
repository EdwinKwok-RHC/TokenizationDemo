namespace TimedScopeMinimizedTokenizationDemo.Tokenization;

public class TokenCleanupService : IHostedService
{
    private readonly ITokenizationService _tokenizationService;
    private Timer _timer;

    public TokenCleanupService(ITokenizationService tokenizationService)
    {
        _tokenizationService = tokenizationService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CleanupTokens, null, TimeSpan.Zero, TimeSpan.FromHours(1)); // Cleanup every hour
        return Task.CompletedTask;
    }

    private void CleanupTokens(object state)
    {
        _tokenizationService.CleanupExpiredTokens();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
}