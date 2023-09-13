using SimpleDB;
using DocoptNet;

IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>("chirp_cli_db.csv");

const string usage = @"Chirp

Usage:
  chirp read <limit>
  chirp cheep <message>
";

var arguments = new Docopt().Apply(usage, args, exit: true)!;

if (arguments["cheep"].IsTrue) {
    database.Store(new Cheep(Environment.UserName, args[1], DateTimeOffset.Now.ToUnixTimeSeconds()));
} else if(arguments["read"].IsTrue) {
    Userinterface.PrintCheeps(database.Read());
}