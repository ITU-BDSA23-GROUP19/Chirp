using SimpleDB;
using DocoptNet;

IDatabase<Cheep> database = new CSVDatabase<Cheep>("../Resources/database.csv");

const string usage = @"Chirp commands

Usage:
  chirp read [--limit]
  chirp cheep (<message>)
  chirp (-h | -help)

Options:
  -h --help     Show this screen. 
  --limit       Cheeps shown [default: 5]
";

var arguments = new Docopt().Apply(usage, args, exit: true)!;

if (arguments["cheep"].IsTrue) {
    database.Store(new Cheep(arguments["<message>"].ToString()));
} else if(arguments["read"].IsTrue) {
    if (arguments["--limit"].IsNullOrEmpty) {
        Userinterface.PrintCheeps(database.Read());
    } else {
        Userinterface.PrintCheeps(database.Read(arguments["--limit"].AsInt));
    }
}