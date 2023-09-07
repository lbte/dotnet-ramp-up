using TimeZonesBackgroundService;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = ".NET Time Zones Service";
    })
    .ConfigureServices(services =>
    {
        // services.AddSingleton<TimeService>();
        // services.AddTransient<TimeService>();
        services.AddScoped<TimeService>();
        services.AddHostedService<WindowsBackgroundService>();
    })
    .Build();

await host.RunAsync();
