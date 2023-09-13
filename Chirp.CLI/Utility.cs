public class Utility {
    public static DateTime TimestampToTime(long timestamp) {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToLocalTime();
    }
}