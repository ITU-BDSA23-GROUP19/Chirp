using System.Net.Http.Json;

using DocoptNet;

using HttpClient client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5250/");

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
  Cheep cheep = new Cheep(Environment.UserName, arguments["<message>"].ToString(), DateTimeOffset.Now.ToUnixTimeSeconds());
  JsonContent content = JsonContent.Create(cheep);
  await client.PostAsync("cheep", content);
}
else if (arguments["read"].IsTrue)
{
  ValueObject limit = arguments["<limit>"];

  if (limit.IsNullOrEmpty | !limit.IsInt)
  {
    IEnumerable<Cheep>? cheeps = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");

    if (cheeps != null)
    {
      Userinterface.PrintCheeps(cheeps);
    }
  }
  else
  {
    IEnumerable<Cheep>? cheeps = await client.GetFromJsonAsync<IEnumerable<Cheep>>($"cheeps?limit={limit}");

    if (cheeps != null)
    {
      Userinterface.PrintCheeps(cheeps);
    }
  }
}

public record Cheep(string Author, string Message, long Timestamp);