namespace PRFTLatam.OrdersData.Services;

public static class Utils
{
    // extension method that returns the date in ISO format YYYYMMDD
    public static string DateToISOFormat(DateTime date)
    {
        return date.ToString("yyyyMMdd");
    }
}