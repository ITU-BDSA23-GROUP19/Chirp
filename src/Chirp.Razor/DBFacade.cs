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

        // Should probably only be done, if there is not created a database file.
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

    private void ExecuteNonQuery(string filePath)
    {
        SqliteConnection connection = ConnectToDatabase();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = GetResourceFile(filePath);
        command.ExecuteNonQuery();
    }

    // Co-authored-by: ChatGPT
    private string GetResourceFile(string filePath)
    {
        Stream? stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Chirp.Razor.data." + filePath);

        if (stream != null)
        {
            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        throw new ArgumentException("Could not find the resource: " + filePath);
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