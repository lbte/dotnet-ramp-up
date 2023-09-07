using System;
using TimeZoneConverter;
using System.Collections.ObjectModel;

namespace ContosoWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private static List<TimeZones> timeZonesList = new(){
        new TimeZones("Bogota", "America/Bogota", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("America/Bogota"))),
        new TimeZones("Chicago", "America/Chicago", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("America/Chicago"))),
        new TimeZones("Argentina", "America/Argentina/Buenos_Aires", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("America/Argentina/Buenos_Aires"))),
        new TimeZones("Detroit", "America/Detroit", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("America/Detroit"))),
        new TimeZones("London", "Europe/London", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("Europe/London")))
    };
    private static ReadOnlyCollection<TimeZones> timeZonesCollection = new ReadOnlyCollection<TimeZones>(timeZonesList);

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string bogota = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(0));
                string chicago = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(1));
                string argentina = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(2));
                string detroit = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(3));
                string london = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(4));

                _logger.LogInformation("{bogota}{chicago}{argentina}{detroit}{london}", bogota, chicago, argentina, detroit, london);

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }   
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            Environment.Exit(1);
        }
    }

    private string GetTimeZoneDisplayed(TimeZones timeZone)
    {
        return $"City: {timeZone.city}\nTimeZone: {timeZone.timeZoneName}\nTime: {timeZone.timeZoneTime.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFzzz")}\n\n"; 
    }
}

readonly record struct TimeZones(string city, string timeZoneName, DateTime timeZoneTime);
