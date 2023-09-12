using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

void Read() {
    using StreamReader reader = new StreamReader("chirp_cli_db.csv");
    using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    
    IEnumerable<Cheep> records = csv.GetRecords<Cheep>();
    Userinterface.PrintCheeps(records);
}

void Cheep() {
    List<Cheep> records = new List<Cheep> { new Cheep (Environment.UserName, args[1], DateTimeOffset.Now.ToUnixTimeSeconds()) };
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };

    using StreamWriter writer = new StreamWriter("chirp_cli_db.csv", true);
    using CsvWriter csv = new CsvWriter(writer, config);
    csv.WriteRecords(records);
}

if (args.Length == 0) {
    Console.WriteLine("No command used: Use 'dotnet run -- help' to see commands.");
} else if (args[0] == "help") {
    Console.WriteLine("Use 'dotnet run -- read' to read cheeps.");
    Console.WriteLine("Use 'dotnet run -- cheep \"message\"' to post cheep.");
} else if (args[0] == "read") {
    Read();
} else if (args[0] == "cheep") {
    Cheep();
} else {
    Console.WriteLine("Invalid command used: Use 'dotnet run -- help' to see valid commands.");
}

public record Cheep(string Author, string Message, long Timestamp);