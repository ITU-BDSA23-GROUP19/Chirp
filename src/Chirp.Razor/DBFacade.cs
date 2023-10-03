using System.Data;

using Microsoft.Data.Sqlite;

public class DBFacade
{
    private readonly string _path;

    public DBFacade(string path)
    {
        _path = path;
    }

    /*
        public void Facade()
        {
            SqliteConnection connection = Connect();


            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                var dataRecord = (IDataRecord)reader;
                for (int i = 0; i < dataRecord.FieldCount; i++)
                    Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
                // for documentation on how to retrieve complete columns from query results
                Object[] values = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(values);
                for (int i = 0; i < fieldCount; i++)
                    Console.WriteLine($"{reader.GetName(i)}: {values[i]}");
            }
        }
    */


    public List<CheepViewModel> GetCheeps()
    {
        SqliteDataReader reader = RunQuery(@"SELECT username, text, pub_date
                                             FROM message m
                                             JOIN user u on m.author_id = u.user_id;");

        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        while (reader.Read())
        {
            // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
            var dataRecord = (IDataRecord)reader;
            for (int i = 0; i < dataRecord.FieldCount; i++)
                Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

            // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
            // for documentation on how to retrieve complete columns from query results
            Object[] values = new Object[reader.FieldCount];
            int fieldCount = reader.GetValues(values);
            for (int i = 0; i < fieldCount; i++)
                Console.WriteLine($"{reader.GetName(i)}: {values[i]}");
        }

        return cheeps;
    }

    public List<CheepViewModel> GetAuthorCheeps(string author)
    {
        throw new Exception();
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
}