public class Utility
{
    public static string TimestampToDateTime(long timestamp)
    {
        var time = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToLocalTime();
        return time.ToString("MM/dd/yyyy HH:mm:ss");
    }
}