namespace test;

public class ChirpTest
{
    [Fact]
    public void timestampToTimeTest(){
        //arrange
        

        //act
        var result = TimestampToDateTime(1690891760);

        //assert
        Assert.Equals(result, "08/01/2023 14:09:20");


    }

    [Fact]
    public void CheepGettersTest(){
//should there be different tests using the two different constructors for cheep??
        //arrange
        var cheep = Cheep("ThisAuthor", "A message here", 1690891760);

        //act
        var authorResult = cheep.Author();
        var messageResult = cheep.Message();
        var timestampResult = cheep.Timestamp();

        //assert
        Assert.Equals(authorResult, "ThisAuthor");
        Assert.Equals(messageResult, "A message here");
        Assert.Equals(timestampResult, 1690891760);
    }

    [Fact]
    public void CheepDisplayTest(){
        //arrange
        var cheep = Cheep("ThisAuthor", "A message here", 1690891760);
        //act
        var result = cheep.Display();
        //assert
        Assert.Equals(result, "ThisAuthor @ 08/01/2023 14:09:20: A message here");
    }


}