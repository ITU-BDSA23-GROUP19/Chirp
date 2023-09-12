using SimpleDB;

CSVDatabase<Cheep> csvDb = new CSVDatabase<Cheep>("chirp_cli_db.csv");

    if (args.Length == 0)
    {
        Console.WriteLine("No command used: Use 'dotnet run -- help' to see commands.");
        Console.WriteLine();
        return;
    }

    if (args[0] == "help")
    {
        Console.WriteLine("Use 'dotnet run -- read' to read cheeps.");
        Console.WriteLine("Use 'dotnet run -- cheep \"message\"' to post cheep.");

    }
    else if (args[0] == "read")
    {
        Read();
    }

    else if (args[0] == "cheep")
    {
        var record = new Cheep(Environment.UserName, args[1], DateTimeOffset.Now.ToUnixTimeSeconds());
        Cheep(record);

    }
    else
    {
        Console.WriteLine("Invalid command used: Use 'dotnet run -- help' to see valid commands.");
    }

    Console.WriteLine();



string TimestampToTime(long timestamp) {
    return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToLocalTime().ToString();
    }

void Read() {
    var records = csvDb.Read();
    foreach (Cheep record in records) { Console.WriteLine($"{record.Author} @ {TimestampToTime(record.Timestamp)} : {record.Message}"); }}

void Cheep(Cheep record){
    csvDb.Store(record);
    }



public record Cheep(string Author, string Message, long Timestamp);

