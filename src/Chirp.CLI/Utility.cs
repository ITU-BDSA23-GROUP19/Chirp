using System.Globalization;

public class Utility
{
    public static string TimestampToDateTime(long timestamp)
    {
        var utc = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToUniversalTime();  
        var cet = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");           
        return TimeZoneInfo.ConvertTimeFromUtc(utc, cet).ToString(CultureInfo.InvariantCulture);
    }
}