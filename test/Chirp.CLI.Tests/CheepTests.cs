namespace Chirp.CLI.Tests
{
    public class ChirpTest
    {
        [Fact]
        public void timestampToTimeTest()
        {
            //arrange

            //act
            var result = Utility.TimestampToDateTime(1690891760);

            //assert
            Assert.Equal("08/01/2023 14:09:20", result.ToString());
        }

        [Fact]
        public void CheepGettersTest()
        {
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

        [Fact]
        public void CheepDisplayTest()
        {
            //arrange
            var cheep = new Cheep("ThisAuthor", "A message here", 1690891760);

            //act
            var result = cheep.Display();

            //assert
            Assert.Equal("ThisAuthor @ 08/01/2023 14:09:20: A message here", result);
        }
    }
}