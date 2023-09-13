namespace SimpleDB;

using CsvHelper;
using System.Globalization;

public class CSVDatabase<T> : IDatabaseRepository<T> {
    private readonly string _file;

    public CSVDatabase(string file) {
        _file = file;
    }

    public IEnumerable<T> Read() {
        using StreamReader reader = new StreamReader(_file);
        using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        return csv.GetRecords<T>().ToList();
    }
 
    public void Store(T record) {
        using StreamWriter writer = new StreamWriter(_file, true);
        using CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        
        csv.WriteRecord(record);
        csv.NextRecord();
    }
}
