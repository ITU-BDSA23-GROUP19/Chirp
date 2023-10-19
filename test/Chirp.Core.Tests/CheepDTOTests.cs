using System.Runtime.InteropServices;

namespace Chirp.Core.Tests;

public class CheepDTOTests
{
    [Theory]
    [InlineData("Rasmus", "Hej, velkommen til kurset.", "2023-08-01 13:08:28")]
    [InlineData("Helge", "Hello, BDSA students!", "2023-08-01 12:16:48")]
    [InlineData("Johnnie Calixto", "What do you think so meanly of him?", "2023-08-01 13:13:56")]
    public void CanCreateCheepDTO(string author, string text, string timeStamp)
    {
        // Arrange
        CheepDTO cheep = new CheepDTO(author, text, timeStamp);

        // Act
        string Author = cheep.Author;
        string Text = cheep.Text;
        string TimeStamp = cheep.TimeStamp;

        // Assert
        Assert.Equal(author, Author);
        Assert.Equal(text, Text);
        Assert.Equal(timeStamp, TimeStamp);
    }
}