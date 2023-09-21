using CsvHelper;

using System.Globalization;

namespace SimpleDB
{
    public class CSVDatabase<T> : IDatabase<T>
    {
        private static CSVDatabase<T>? instance = null;
        private readonly string _path;

        private CSVDatabase()
        {
            //goes out and looks after the folder containing the database
            //needed for testing, as these run from a different directory than the program
            //made with help from chatGPT
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            while (currentDirectory != null)
            {
                var potentialPath = Path.Combine(currentDirectory, "data", "database.csv");
                if (File.Exists(potentialPath))
                {
                    _path = potentialPath;
                    break;
                }
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }
        }

        public static CSVDatabase<T> GetInstance()
        {
            if (instance == null)
            {
                instance = new CSVDatabase<T>();
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