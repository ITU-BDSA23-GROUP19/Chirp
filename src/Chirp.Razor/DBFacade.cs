using System.Data;

using Microsoft.Data.Sqlite;

public class DBFacade
{
    private readonly string _dataSource;

    public DBFacade()
    {
        string? value = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        if (string.IsNullOrWhiteSpace(value))
        {
            _dataSource = "/tmp/chirp.db";
        }
        else
        {
            _dataSource = value;
        }

        if (!File.Exists(_dataSource))
        {
            ExecuteDatabaseResource("Chirp.Razor.data.schema.sql");
            ExecuteDatabaseResource("Chirp.Razor.data.dump.sql");
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

    private void ExecuteDatabaseResource(string resourcePath)
    {
        SqliteConnection connection = ConnectToDatabase();

        Stream stream = Utility.GetResourceStream(resourcePath);
        StreamReader reader = new StreamReader(stream);

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = reader.ReadToEnd();
        command.ExecuteNonQuery();
    }

    private SqliteConnection ConnectToDatabase()
    {
        SqliteConnection connection = new SqliteConnection($"Data Source={_dataSource}");
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
                                          Utility.UnixTimeStampToDateTimeString(reader.GetInt32(2))));
        }

        return cheeps;
    }
}