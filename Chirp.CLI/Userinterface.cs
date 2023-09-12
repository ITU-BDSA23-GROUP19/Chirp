public static class Userinterface{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps){
        foreach (Cheep cheep in cheeps) {
            Console.WriteLine($"{cheep.Author} @ {TimestampToTime(cheep.Timestamp)}: {cheep.Message}");
        }
    }

    private static DateTime TimestampToTime(long timestamp) {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime.ToLocalTime();
    }
}