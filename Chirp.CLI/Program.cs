﻿using SimpleDB;
using DocoptNet;

IDatabase<Cheep> database = new CSVDatabase<Cheep>("database.csv");

const string usage = @"Chirp commands

Usage:
  chirp read <limit>
  chirp cheep <message>
  chirp (-h | -help)
";

var arguments = new Docopt().Apply(usage, args, exit: true)!;

if (arguments["cheep"].IsTrue) {
    database.Store(new Cheep(args[1]));
} else if(arguments["read"].IsTrue) {
    Userinterface.PrintCheeps(database.Read());
}