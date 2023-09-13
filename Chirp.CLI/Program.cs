using SimpleDB;
using DocoptNet;

const string usage = @"Chirp CLI

Usage:
  chirp read <limit>
  chirp cheep <message>
  chirp (-h | --help)
";

IDatabaseRepository<Cheep> cheepDB = new CSVDatabase<Cheep>("chirp_cli_db.csv");

IDictionary<string, ValueObject> arguments = new Docopt().Apply(usage, args, exit: true)!;
if (arguments["cheep"].IsTrue) {
    Cheep cheep = new Cheep(Environment.UserName, args[1], DateTimeOffset.Now.ToUnixTimeSeconds());
    cheepDB.Store(cheep);
} else if(arguments["read"].IsTrue) {
    IEnumerable<Cheep> cheeps = cheepDB.Read();
    Userinterface.PrintCheeps(cheeps);
}

public record Cheep(string Author, string Message, long Timestamp);