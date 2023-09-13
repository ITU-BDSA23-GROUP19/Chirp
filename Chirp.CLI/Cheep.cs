public class Cheep {
    public Cheep(string message) {
        Author = Environment.UserName;
        Message = message;
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public string Author { get; }
    public string Message { get; }
    public long Timestamp { get; }
}