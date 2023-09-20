using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestPlatform.TestHost;

using SimpleDB;

namespace test;

public class ChirpTest
{
    /*[Fact]
    public void timestampToTimeTest()
    {
        //arrange


        //act
        var result = Utility.TimestampToDateTime(1690891760);

        //assert
        Assert.Equal("08/01/2023 14:09:20", result.ToString());


    }*/

    [Fact]
    public void CheepGettersTest()
    {
        //should there be different tests using the two different constructors for cheep??
        //arrange
        var cheep = new Cheep("ThisAuthor", "A message here", 1690891760);

        //act
        var authorResult = cheep.Author;
        var messageResult = cheep.Message;
        var timestampResult = cheep.Timestamp;

        //assert
        Assert.Equal("ThisAuthor", authorResult);
        Assert.Equal("A message here", messageResult);
        Assert.Equal(1690891760, timestampResult);
    }

    /*[Fact]
    public void CheepDisplayTest()
    {
        //arrange
        var cheep = new Cheep("ThisAuthor", "A message here", 1690891760);
        //act
        var result = cheep.Display();
        //assert
        Assert.Equal("ThisAuthor @ 08/01/2023 14:09:20: A message here", result);
    }*/

    [Fact]
    public void ReadCheeps() //end to end test
    {
        //arrange
        IDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance();

        TextWriter originalConsoleOut = Console.Out; //store the original console output

        using (StringWriter stringWriter = new StringWriter())
        {
            Console.SetOut(stringWriter);

            //act
            //these could be called as Program.Main with arguments to stimulate a user
            //but it doesnt work for some reason - my guess is it doesnt parse the arguments correctly
            //and i cant seem to fix it right now

            //writing a test-cheep
            database.Store(new Cheep("This is a test"));
            //reading cheeps
            Userinterface.PrintCheeps(database.Read());

            Console.SetOut(originalConsoleOut);
            string capturedOutput = stringWriter.ToString();

            //assert
            Assert.Contains("Hello, BDSA students!", capturedOutput);
            Assert.Contains("Welcome to the course!", capturedOutput);
            Assert.Contains("ropf", capturedOutput);
            Assert.Contains("rnie", capturedOutput);
            Assert.Contains("This is a test", capturedOutput);
        }

    }


}