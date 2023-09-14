namespace test;

public class ChirpTest
{
    [Fact]
    public void timestampToTimeTest(){
        //arrange
        var result = TimestampToDateTime(1690891760);

        //act

        //assert
        Assert.Equals(result, "08/01/2023 14:09:20")


    }
}