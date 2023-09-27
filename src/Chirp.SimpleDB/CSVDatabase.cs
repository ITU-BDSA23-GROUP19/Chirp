using System.Globalization;

using CsvHelper;

namespace Chirp.SimpleDB
{
    public class CSVDatabase<T> : IDatabase<T>
    {
        private static CSVDatabase<T>? instance = null;
        private readonly string _path;

        private CSVDatabase(string path)
        {
            _path = path;
        }

        public static CSVDatabase<T> GetInstance(string path)
        {
            if (instance == null)
            {
                instance = new CSVDatabase<T>(path);
            }

            return instance;
        }

        public IEnumerable<T> Read(int? limit = null)
        {
            using StreamReader reader = new StreamReader(_path);
            using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            List<T> records = csv.GetRecords<T>().ToList();
            if (limit.HasValue)
            {
                limit = int.Min(limit.Value, records.Count);
                return records.GetRange(records.Count - limit.Value, limit.Value);
            }
            else
            {
                return records;
            }
        }

        public void Store(T record)
        {
            using StreamWriter writer = new StreamWriter(_path, true);
            using CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }
}