namespace Chirp.CLI.Tests
{
    public class ChirpTests
    {
        [Fact]
        public void CheepGettersTest()
        {
            //arrange
            Cheep cheep = new Cheep("ThisAuthor", "A message here", 1690891760);

            //act
            string authorResult = cheep.Author;
            string messageResult = cheep.Message;
            long timestampResult = cheep.Timestamp;

            //assert
            Assert.Equal("ThisAuthor", authorResult);
            Assert.Equal("A message here", messageResult);
            Assert.Equal(1690891760, timestampResult);
        }

        [Fact]
        public void CheepDisplayTest()
        {
            //arrange
            Cheep cheep = new Cheep("ThisAuthor", "A message here", 1690891760);

            //act
            string result = cheep.Display();

            //assert
            Assert.Equal("ThisAuthor @ 08/01/2023 14:09:20: A message here", result);
        }
    }
}