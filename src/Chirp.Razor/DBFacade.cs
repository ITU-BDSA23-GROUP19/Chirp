using System.Data;
using System.Reflection;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;

public class DBFacade
{
    private readonly string _connectionString;

    public DBFacade()
    {
        SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder
        {
            DataSource = GetDataSource()
        };

        _connectionString = builder.ToString();

        ExecuteNonQuery("schema.sql");
        ExecuteNonQuery("dump.sql");
    }

    private string GetDataSource()
    {
        string? value = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        if (string.IsNullOrWhiteSpace(value))
        {
            return Path.GetTempPath() + "chirp.db";
        }
        else
        {
            return value;
        }
    }

    // https://stackoverflow.com/questions/7387085/how-to-read-an-entire-file-to-a-string-using-c
    private void ExecuteNonQuery(string filePath)
    {
        SqliteConnection connection = ConnectToDatabase();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = GetResourceFile(filePath);
        command.ExecuteNonQuery();
    }

    private string GetResourceFile(string filePath)
    {
        Stream stream = new EmbeddedFileProvider(GetType().GetTypeInfo().Assembly, "Chirp.Razor.data").GetFileInfo(filePath).CreateReadStream();
        StreamReader streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
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
        SqliteConnection connection = new SqliteConnection(_connectionString);
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