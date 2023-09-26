using Chirp.SimpleDB;

using DocoptNet;

IDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance("../../data/database.csv");

const string usage = @"Chirp

Usage:
    chirp read [<limit>]
    chirp cheep (<message>)
    chirp (-h | --help)

Options:
    -h --help     Show this screen.
";

IDictionary<string, ValueObject> arguments = new Docopt().Apply(usage, args, exit: true)!;

if (arguments["cheep"].IsTrue)
{
  database.Store(new Cheep(Environment.UserName, arguments["<message>"].ToString(), DateTimeOffset.Now.ToUnixTimeSeconds()));
}
else if (arguments["read"].IsTrue)
{
  ValueObject limit = arguments["<limit>"];

  if (limit.IsNullOrEmpty | !limit.IsInt)
  {
    Userinterface.PrintCheeps(database.Read());
  }
  else
  {
    Userinterface.PrintCheeps(database.Read(limit.AsInt));
  }
}

public record Cheep(string Author, string Message, long Timestamp);