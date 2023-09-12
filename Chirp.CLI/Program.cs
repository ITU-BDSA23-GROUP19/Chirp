using SimpleDB;

IDatabaseRepository<Cheep> csvDb = new CSVDatabase<Cheep>("chirp_cli_db.csv");

if (args.Length == 0) {
    Console.WriteLine("No command used: Use 'dotnet run -- help' to see commands.");
    return;
}

if (args[0] == "read") {
    var records = csvDb.Read();
    Userinterface.PrintCheeps(records);
} else if (args[0] == "cheep") {
    Cheep record = new Cheep(Environment.UserName, args[1], DateTimeOffset.Now.ToUnixTimeSeconds());
    csvDb.Store(record);
}

public record Cheep(string Author, string Message, long Timestamp);