using Chirp.CLI;

namespace Chirp.SimpleDB.Tests
{
    public class CSVDatabaseTests
    {
        [Fact]
        public void CSVDatabase_InstanceExists()
        {
            //Arrange
            var database = CSVDatabase<Type>.GetInstance();
            //Act

            //Assert
            Assert.NotNull(database);
        }

        [Fact]
        public void Read_Returns_Records()
        {
            //Arrange
            Cheep cheep = new Cheep("This is a randomized tweet!:D");
            var database = CSVDatabase<Cheep>.GetInstance();
            //Act
            database.Store(cheep);
            var readResult = database.Read();
            //Assert
            Assert.Contains(cheep, readResult);

        }

        [Fact]
        public void Store_Returns_Equal_Record()
        {
            //Arrange
            Cheep cheep = new Cheep("Author", "This is another randomized cheep, u nuget.", 029394848);
            var database = CSVDatabase<Cheep>.GetInstance();
            //Act
            database.Store(cheep);
            //Assert
            Assert.Contains(cheep, database.Read());
        }
    }
}