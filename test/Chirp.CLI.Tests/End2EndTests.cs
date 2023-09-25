using System.Diagnostics;

using Chirp.SimpleDB;

namespace Chirp.CLI.Tests
{
    public class End2EndTests
    {
        [Fact]
        public void ReadCheeps() //end to end test
        {
            //arrange
            IDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance("../../data/database.csv");

            //act
            string output = "";
            using (var process = new Process())
            {
                process.StartInfo.FileName = "/usr/bin/dotnet";
                process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI.dll read";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                output = reader.ReadToEnd();
                process.WaitForExit();
            }
            string fstCheep = output.Split("\n")[0];

            //assert
            Assert.StartsWith("ropf", fstCheep);
            Assert.EndsWith("Hello, BDSA students!", fstCheep);
        }

        [Fact]
        public void WriteCheeps() //end to end test
        {
            //arrange
            IDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance("../../data/database.csv");

            //act
            string output = "";

            //writing first:
            using (var process = new Process())
            {
                process.StartInfo.FileName = "/usr/bin/dotnet";
                process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI.dll cheep \"This is a test\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.Start();
                process.WaitForExit();
            }

            //now we read it to see if we succesfully wrote:
            using (var process = new Process())
            {
                process.StartInfo.FileName = "/usr/bin/dotnet";
                process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI.dll read";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                output = reader.ReadToEnd();
                process.WaitForExit();
            }
            var cheeps = output.Split("\n");
            var lastCheep = cheeps[cheeps.Length - 2];

            //assert
            Assert.Contains("This is a test", lastCheep);
        }
    }
}