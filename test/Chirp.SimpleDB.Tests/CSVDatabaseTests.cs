namespace Chirp.SimpleDB.Tests
{
    public class CSVDatabaseTests
    {
        [Fact]
        public void CSVDatabase_InstanceExists()
        {
            //Arrange
            IDatabase<Type> database = CSVDatabase<Type>.GetInstance("../../../../../data/testDatabase.csv");

            //Assert
            Assert.NotNull(database);
        }

        [Fact]
        public void Read_Returns_Records()
        {
            //Arrange
            Cheep cheep = new Cheep(Environment.UserName, "lets see if it finds the databse", DateTimeOffset.Now.ToUnixTimeSeconds());
            IDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance("../../../../../data/testDatabase.csv");

            //Act
            database.Store(cheep);
            IEnumerable<Cheep> readResult = database.Read();

            //Assert
            Assert.Contains(cheep, readResult);

        }

        [Fact]
        public void Store_Returns_Equal_Record()
        {
            //Arrange
            Cheep cheep = new Cheep("Author", "This is another randomized cheep, u nuget.", 029394848);
            IDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance("../../../../../data/testDatabase.csv");

            //Act
            database.Store(cheep);

            //Assert
            Assert.Contains(cheep, database.Read());
        }
    }
}