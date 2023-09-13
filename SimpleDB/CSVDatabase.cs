namespace SimpleDB;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public class CSVDatabase<T> : IDatabaseRepository<T> {
    private readonly string file;

    public CSVDatabase(string file) {
        this.file = file;
    }

    public IEnumerable<T> Read() {
        using StreamReader reader = new StreamReader(file);
        using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        IEnumerable<T> records = csv.GetRecords<T>();
        foreach (T record in records) {
            yield return record;
        }
    }
 
    public void Store(T record) {
        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            HasHeaderRecord = false
        };
        
        using StreamWriter writer = new StreamWriter(file, true);
        using CsvWriter csv = new CsvWriter(writer, config);
        csv.WriteRecord(record);
        csv.NextRecord();
    }
}
