using Chirp.SimpleDB;
using Chirp.CLI;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabase<Cheep> db = CSVDatabase<Cheep>.GetInstance();


app.MapPost("/cheep", (Cheep cheep) => { db.Store(cheep); });
app.MapGet("/cheeps", () => { JsonSerializer.Serialize(db.Read()); });


app.Run();
