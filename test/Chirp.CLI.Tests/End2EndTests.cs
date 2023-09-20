using Chirp.SimpleDB;
using Chirp.CLI;

namespace Chirp.CLI.Tests
{
    public class End2EndTests
    {
        [Fact]
        public void ReadCheeps() //end to end test
        {
            //arrange
            IDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance();

            TextWriter originalConsoleOut = Console.Out; //store the original console output

            using StringWriter stringWriter = new StringWriter();
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