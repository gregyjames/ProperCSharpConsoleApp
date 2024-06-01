using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App;

public class SampleService: IHostedService
{
    private readonly ILogger<SampleService> _logger;
    private readonly IHostApplicationLifetime _appLifetime;

    public SampleService(ILogger<SampleService> logger, IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _appLifetime = appLifetime;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting service...");
        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(5000);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            }, cancellationToken);
        });
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Service ended.");
    }
}