using System.Data;

using Microsoft.Data.Sqlite;

public class DBFacade
{
    private readonly string _path;

    public DBFacade()
    {
        string? value = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        if (value == null)
        {
            _path = "/tmp/chirp.db";
        }
        else
        {
            _path = value;
        }
    }

    public List<CheepViewModel> GetCheeps()
    {
        SqliteConnection connection = ConnectToDatabase();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT username, text, pub_date
            FROM message m
            JOIN user u on m.author_id = u.user_id;
        ";

        SqliteDataReader reader = command.ExecuteReader();

        return RetrieveCheeps(reader);
    }

    public List<CheepViewModel> GetAuthorCheeps(string author)
    {
        SqliteConnection connection = ConnectToDatabase();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT username, text, pub_date
            FROM message m
            JOIN user u on m.author_id = u.user_id
            WHERE username = $author;
        ";
        command.Parameters.AddWithValue("$author", author);

        SqliteDataReader reader = command.ExecuteReader();

        return RetrieveCheeps(reader);
    }

    private SqliteConnection ConnectToDatabase()
    {
        SqliteConnection connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        return connection;
    }

    private List<CheepViewModel> RetrieveCheeps(SqliteDataReader reader)
    {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        while (reader.Read())
        {
            cheeps.Add(new CheepViewModel(reader.GetString(0),
                                          reader.GetString(1),
                                          UnixTimeStampToDateTimeString(reader.GetInt32(2))));
        }

        return cheeps;
    }

    private string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}