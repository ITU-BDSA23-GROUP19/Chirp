using System.Globalization;

public class Utility
{
    public static string TimestampToDateTime(long timestamp)
    {
        var utc = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToUniversalTime();  
        Console.WriteLine(utc + "Heyyyyy");
        var cet = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        Console.WriteLine(cet + "Heyyyyy");              
        return TimeZoneInfo.ConvertTimeFromUtc(utc, cet).ToString(CultureInfo.InvariantCulture);
    }
}