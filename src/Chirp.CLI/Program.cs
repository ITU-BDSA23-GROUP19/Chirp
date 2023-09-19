using SimpleDB;
using DocoptNet;
using Chirp.CLI;

IDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance();

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
  database.Store(new Cheep(arguments["<message>"].ToString()));
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