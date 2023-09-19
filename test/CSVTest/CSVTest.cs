using System.ComponentModel.Design;

using SimpleDB;

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

    public void Read_Returns_Recrods()
    {
        //Arrange
        var cheep = "This is a randomized tweet!:D";

        //Act
        var readResult = cheep.Read();
        //Assert
        Assert.Contains(cheep, readResult);
    }

    public void Store_Returns_Equal_Record()
    {
        //Arrange
        var cheep = "This is another randomized cheep, u nuget.";
        //Act
        var storeResult = Store(cheep);
        //Assert
    }


    //Integration Tests
    [Fact]
    public void CSVDataLibrary_StorageTest()
    //M
    {
        //Arrange
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