public class Cheep {
    public Cheep(string message) {
        Author = Environment.UserName;
        Message = message;
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public string Author { get; }
    public string Message { get; }
    public long Timestamp { get; }

    public override string ToString() {
        return $"{Author} @ {TimestampToTime(Timestamp)}: {Message}";
    }

    private DateTime TimestampToTime(long timestamp) {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToLocalTime();
    }
}