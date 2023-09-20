using Chirp.CLI;

namespace Chirp.CLI.Tests
{
    public class UtilityTests
    {
        [Fact]
        public void timestampToTimeTest()
        {
            //arrange
            long timestamp = 1690891760;

            //act
            string result = Utility.TimestampToDateTime(timestamp);

            //assert
            Assert.Equal("08/01/2023 14:09:20", result);
        }
    }
}