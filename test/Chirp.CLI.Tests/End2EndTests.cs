using System.Diagnostics;

namespace Chirp.CLI.Tests
{
    public class End2EndTests
    {
        [Fact]
        public void ReadCheeps()
        {
            //arrange
            string output = "";

            //act
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net7.0/Chirp.dll read";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

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
        public void WriteCheeps()
        {
            //arrange
            string output = "";

            //act
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net7.0/Chirp.dll cheep \"This is a test\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.Start();
                process.WaitForExit();
            }

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net7.0/Chirp.dll read";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                StreamReader reader = process.StandardOutput;
                output = reader.ReadToEnd();
                process.WaitForExit();
            }

            string[] cheeps = output.Split("\n");
            string lastCheep = cheeps[cheeps.Length - 2];

            //assert
            Assert.Contains("This is a test", lastCheep);
        }
    }
}