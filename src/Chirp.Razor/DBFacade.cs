using System.Data;

using Microsoft.Data.Sqlite;

public class DBFacade
{
    private readonly string _path;

    public DBFacade()
    {
        string? value = Environment.GetEnvironmentVariable("CHIRPDBPATH");
        if (value == null) {
            _path = "/tmp/chirp.db";
        } else {
            _path = value;
        }
    }

    public List<CheepViewModel> GetCheeps()
    {
        SqliteDataReader reader = RunQuery(@"SELECT username, text, pub_date
                                             FROM message m
                                             JOIN user u on m.author_id = u.user_id;");

        return RetrieveCheeps(reader);
    }

    public List<CheepViewModel> GetAuthorCheeps(string author)
    {
        SqliteDataReader reader = RunQuery(@$"SELECT username, text, pub_date
                                             FROM message m
                                             JOIN user u on m.author_id = u.user_id
                                             WHERE username = '{author}';");

        return RetrieveCheeps(reader);
    }

    private SqliteConnection ConnectToDB()
    {
        SqliteConnection connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();
        return connection;
    }

    private SqliteDataReader RunQuery(string query)
    {
        SqliteConnection connection = ConnectToDB();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = query;
        SqliteDataReader reader = command.ExecuteReader();
        return reader;
    }

    private List<CheepViewModel> RetrieveCheeps(SqliteDataReader reader) {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        
        while (reader.Read())
        {
            cheeps.Add(new CheepViewModel(reader.GetString(0), 
                                          reader.GetString(1), 
                                          CheepService.UnixTimeStampToDateTimeString(reader.GetInt32(2))));
        }

        return cheeps;
    }
}