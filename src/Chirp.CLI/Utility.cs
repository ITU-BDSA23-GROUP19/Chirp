using System.Globalization;

public class Utility
{
    public static string TimestampToDateTime(long timestamp)
    {
        var utc = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToUniversalTime();  // this is for some reason offset by -2 hours
        var cet = TimeZoneInfo.FindSystemTimeZoneById("Russia Time Zone 3");                 // To offset it by +4 hours, to CET time
        return TimeZoneInfo.ConvertTimeFromUtc(utc, cet).ToString(CultureInfo.InvariantCulture);
    }
}