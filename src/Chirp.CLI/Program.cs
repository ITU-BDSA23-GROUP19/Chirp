using System.Net.Http.Json;

using Chirp.SimpleDB;

using CsvHelper.Expressions;

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

//Create an HTTP called object
//BaseURL when deployed to azure should be 'https://bdsagroup19chirpremotedb.azurewebsites.net/'
var baseURL = "http://localhost:5250";
using HttpClient client = new();
client.BaseAddress = new Uri(baseURL);

IDictionary<string, ValueObject> arguments = new Docopt().Apply(usage, args, exit: true)!;
// Concurrent execution
// first HTTP request
var postTask = client.PostAsJsonAsync<Cheep>(baseURL, new Cheep(Environment.UserName, arguments["<message>"].ToString(), DateTimeOffset.Now.ToUnixTimeSeconds()));

//var getResponse = await getTask;

if (arguments["cheep"].IsTrue)
{
  var postResponse = await postTask;
}
else if (arguments["read"].IsTrue)
{
  ValueObject limit = arguments["<limit>"];

  //making a string to end the url with. ? is symbol for beginning of parameters
  //limit equals whatever number is chosen by user
  string limitString = $"?limit={limit}";

  if (limit.IsNullOrEmpty | !limit.IsInt)
  {
    //HTTP get request to get cheeps WITH limit
    var getTask = client.GetAsync(baseURL + limit);
    var getResponse = await getTask;
    //Userinterface.PrintCheeps(database.Read());
  }
  else
  {
    //HTTP get request to get cheeps WITHOUT limit
    var getTask = client.GetAsync(baseURL);
    var getResponse = await getTask;
    //Userinterface.PrintCheeps(database.Read(limit.AsInt));
  }
}

public record Cheep(string Author, string Message, long Timestamp);