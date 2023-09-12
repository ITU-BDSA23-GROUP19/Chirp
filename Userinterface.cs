public static class Userinterface{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps){
        foreach (Cheep cheep in cheeps) {
            Console.WriteLine($"{cheep.Author} @ {TimestampToTime(cheep.Timestamp)}: {cheep.Message}");
        }
    }

    private static string TimestampToTime(long timestamp) {
        return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToLocalTime().ToString();
    }
}