namespace Chirp.SimpleDB.Tests
{
    public class CSVDatabaseTests
    {
        [Fact]
        public void CSVDatabase_InstanceExists()
        {
            //Arrange
            var database = CSVDatabase<Type>.GetInstance("../../../../../data/testDatabase.csv");
            //Act

            //Assert
            Assert.NotNull(database);
        }

        [Fact]
        public void Read_Returns_Records()
        {
            //Arrange
            Cheep cheep = new Cheep(Environment.UserName, "lets see if it finds the databse", DateTimeOffset.Now.ToUnixTimeSeconds());
            var database = CSVDatabase<Cheep>.GetInstance("../../../../../data/testDatabase.csv");
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
            var database = CSVDatabase<Cheep>.GetInstance("../../../../../data/testDatabase.csv");
            //Act
            database.Store(cheep);
            //Assert
            Assert.Contains(cheep, database.Read());
        }

        //SEND AN HTTP GET REQUEST TO /cheeps ENDPOINT
        //HTTP RESPONSE (STATUS CODE) IS 200
        //RESPONSE BODY CONTAINS LIST OF CHEEP OBJECTS SERIALIZED TO JSON

        [Fact]
        public void Http_GetRequest_Returns_200_And_JSONCheepList()
        {
            //Arrange
            //Act
            //Assert
        }


        ///SEND AN HTTP POST REQUEST TO /cheep ENDPOINT
        //HTTP RESPONSE /STATUS CODE) SHOULD BE 200
        //REQUEST BODY SHOULD CONTAIN A JSON SERIALIZED CHEEP OBJECT
        [Fact]
        public void Http_PostRequest_Returns_200_And_CheepAsJSON()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}