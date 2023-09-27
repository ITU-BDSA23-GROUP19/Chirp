using Chirp.SimpleDB;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance("../../data/database.csv");

app.MapGet("/cheeps", (int? limit) => { return JsonSerializer.Serialize(database.Read(limit)); });
app.MapPost("/cheep", (Cheep cheep) => { database.Store(cheep); });

app.Run();
