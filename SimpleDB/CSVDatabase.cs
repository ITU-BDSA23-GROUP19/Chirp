namespace SimpleDB;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public class CSVDatabase<T> : IDatabaseRepository<T> {
    private readonly string file;

    public CSVDatabase(string file) {
        this.file = file;
    }

    public IEnumerable<T> Read(int? limit = null) {
        using var reader = new StreamReader(file);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<T>();
        foreach (T record in records) {
            yield return record;
        }
    }
 
    public void Store(T record) {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            HasHeaderRecord = false
        };
        
        using var writer = new StreamWriter(file, true);
        using var csv = new CsvWriter(writer, config);
        csv.WriteRecord(record);
        csv.NextRecord();
    }
}
