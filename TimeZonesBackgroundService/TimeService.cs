using TimeZoneConverter;
using System.Collections.ObjectModel;

namespace TimeZonesBackgroundService;

public sealed class TimeService
{
    private static List<TimeZones> timeZonesList = new List<TimeZones>();
    private readonly ILogger<WindowsBackgroundService> _logger;

    public TimeService(ILogger<WindowsBackgroundService> logger)
    {
        _logger = logger;
    }

    private ReadOnlyCollection<TimeZones> CreateTimeZonesCollection(DateTime actualDate)
    {
        timeZonesList.Add(new TimeZones("Bogota", "America/Bogota", TimeZoneInfo.ConvertTime(actualDate, TZConvert.GetTimeZoneInfo("America/Bogota"))));
        timeZonesList.Add(new TimeZones("Chicago", "America/Chicago", TimeZoneInfo.ConvertTime(actualDate, TZConvert.GetTimeZoneInfo("America/Chicago"))));
        timeZonesList.Add(new TimeZones("Argentina", "America/Argentina/Buenos_Aires", TimeZoneInfo.ConvertTime(actualDate, TZConvert.GetTimeZoneInfo("America/Argentina/Buenos_Aires"))));
        timeZonesList.Add(new TimeZones("Detroit", "America/Detroit", TimeZoneInfo.ConvertTime(actualDate, TZConvert.GetTimeZoneInfo("America/Detroit"))));
        timeZonesList.Add(new TimeZones("London", "Europe/London", TimeZoneInfo.ConvertTime(actualDate, TZConvert.GetTimeZoneInfo("Europe/London"))));

        return new ReadOnlyCollection<TimeZones>(timeZonesList);
    }

    public string GetTime(DateTime actualDate)
    {
        var timeZonesCollection = CreateTimeZonesCollection(actualDate);
        string bogota = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(0));
        string chicago = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(1));
        string argentina = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(2));
        string detroit = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(3));
        string london = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(4));

        return $"{bogota}{chicago}{argentina}{detroit}{london}";
    }

    private string GetTimeZoneDisplayed(TimeZones timeZone)
    {
        return $"City: {timeZone.city}\nTimeZone: {timeZone.timeZoneName}\nTime: {timeZone.timeZoneTime.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFzzz")}\n\n"; 
    }

    public async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        try
        {
            int count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                string time;
                DateTime actualDate = new DateTime();
                TimeSpan duration = new TimeSpan(0, 0, 30);

                if (count == 0)
                {
                    actualDate = DateTime.Now;
                    time = GetTime(actualDate);
                }
                else
                {
                    actualDate = actualDate.Add(duration);
                    time = GetTime(actualDate);
                }

                _logger.LogInformation("{time}", time);

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

                count++;
            }   
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            Environment.Exit(1);
        }
    }
}

readonly record struct TimeZones(string city, string timeZoneName, DateTime timeZoneTime);