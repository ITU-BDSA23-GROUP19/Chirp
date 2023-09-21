using System.Globalization;

public class Utility
{
    public static string TimestampToDateTime(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToLocalTime().ToString(CultureInfo.InvariantCulture);
    }
}