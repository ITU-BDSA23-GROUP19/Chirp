public class Cheep {
    public Cheep(string author, string message, long timestamp) {
        Author = author;
        Message = message;
        Timestamp = timestamp;
    }

    public Cheep(string message) {
        Author = Environment.UserName;
        Message = message;
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public string Author { get; }
    public string Message { get; }
    public long Timestamp { get; }

    public override string ToString() {
        return $"{Author} @ {Utility.TimestampToDateTime(Timestamp)}: {Message}";
    }
}