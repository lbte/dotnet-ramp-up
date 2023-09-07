using System;
using Microsoft.Extensions.DependencyInjection;

namespace TimeZonesBackgroundService;

public sealed class WindowsBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WindowsBackgroundService> _logger;
    // private readonly TimeService _timeService;

    public WindowsBackgroundService(IServiceProvider serviceProvider, ILogger<WindowsBackgroundService> logger) // , TimeService timeService
    {
        _serviceProvider = serviceProvider;
        // _timeService = timeService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DoWorkAsync(stoppingToken);
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            TimeService scopedTimeService = scope.ServiceProvider.GetRequiredService<TimeService>();

            await scopedTimeService.DoWorkAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(WindowsBackgroundService)} completed");

        await base.StopAsync(stoppingToken);
    }
}


