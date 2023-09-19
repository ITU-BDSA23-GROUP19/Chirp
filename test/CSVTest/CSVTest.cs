
using System.ComponentModel.Design;
using SimpleDB;
using Chirp.CLI;

namespace CSVTest;

public class CSVTest
{
    //Unit Tests
    public void CSVDatabase_InstanceExists()
    {
        //Arrange
        var database = CSVDatabase<Type>.GetInstance();
        //Act

        //Assert
        Assert.NotNull(database);
    }

    public void Read_Returns_Records()
    {
        //Arrange
        Type cheep = new Cheep("This is a randomized tweet!:D");
        var database = CSVDatabase<Type>.GetInstance();
        //Act
        database.Store(cheep);
        var readResult = database.Read(cheep);
        //Assert
        Assert.Contains(cheep, readResult);

    }

    public void Store_Returns_Equal_Record()
    {
        //Arrange
        Type cheep = new Cheep("This is another randomized cheep, u nuget.");
        var database = CSVDatabase<Type>.GetInstance();
        //Act
        database.Store(cheep);
        var readStored = database.Read(cheep);
        //Assert
        Assert.Contains(cheep, readStored);
    }


    //Integration Tests
    [Fact]
    public void CSVDataLibrary_StorageTest()
    {
        //Arrange
        Type cheep = new Cheep("Hi:)");
        //Act
        //Assert
    }

    public void CSVDataLibrary_CheepTest()
    {
        //Arrange
        //Act
        //Assert
    }
}