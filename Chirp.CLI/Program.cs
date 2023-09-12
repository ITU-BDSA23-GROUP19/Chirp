using SimpleDB;
using DocoptNet;

const string usage = @"Chirp CLI.

Usage:
  chirp read <limit>
  chirp cheep <message>
  chirp (-h | --help)

";

IDatabaseRepository<Cheep> csvDb = new CSVDatabase<Cheep>("chirp_cli_db.csv");

var arguments = new Docopt().Apply(usage, args, exit: true)!;

if(arguments["cheep"].IsTrue) {
    Cheep record = new Cheep(Environment.UserName, args[1], DateTimeOffset.Now.ToUnixTimeSeconds());
    csvDb.Store(record);
} else if(arguments["read"].IsTrue) {
    var records = csvDb.Read();
    Userinterface.PrintCheeps(records);
}

public record Cheep(string Author, string Message, long Timestamp);