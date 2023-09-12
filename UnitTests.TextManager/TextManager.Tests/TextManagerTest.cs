using System.Text.RegularExpressions;
using Xunit;

namespace TextManager.Tests;

public class TextManagerTest
{
    TextManager textManagerGlobal;
    public TextManagerTest()
    {
        textManagerGlobal = new TextManager("Hola hola desde xunit");
    }
    
    [Fact]
    public void CountWords()
    {
        // Arrange
        var textManager = new TextManager("Texto prueba");

        // Act
        var result = textManager.CountWords();

        // Assert
        // Assert.Equal(2, result);
        Assert.True(result > 1);
    }

    [Theory]
    [InlineData("Hola Mundo", 2)]
    [InlineData("", 0)]
    [InlineData("Saludos a todos desde el curso de xunit", 8)]
    public void CountWords_Theory(string text, int expected)
    {
        // Arrange
        var textManager = new TextManager(text);

        // Act
        var result = textManager.CountWords();

        // Assert
        Assert.Equal(expected, result);
        // Assert.True(result > 1);
    }

    [Theory]
    [ClassData(typeof(TextManagerClassData))]
    public void CountWords_ClassData(string text, int expected)
    {
        // Arrange
        var textManager = new TextManager(text);

        // Act
        var result = textManager.CountWords();

        // Assert
        Assert.Equal(expected, result);
        // Assert.True(result > 1);
    }

    [Fact]
    public void CountWords_NotZero()
    {
        // Arrange
        var textManager = new TextManager("Tex");

        // Act
        var result = textManager.CountWords();

        // Assert
        Assert.NotEqual(0, result);
    }

    [Fact]
    public void FindWord()
    {
        // Arrange
        // var textManager = new TextManager("Hola hola desde xunit");

        // Act
        var result = textManagerGlobal.FindWord("Hola", bolUpperLowerCase: true);

        // Assert
        Assert.Contains(0, result); //must be located in the 0 position
    }

    [Fact]
    public void FindWord_Empty()
    {
        // Arrange
        // var textManager = new TextManager("Hola hola desde xunit");

        // Act
        // var result = textManager.FindWord("mundo", bolUpperLowerCase: true);
        var result = textManagerGlobal.FindWord("mundo", bolUpperLowerCase: true);

        // Assert
        Assert.Empty(result); //must be empty
    }

    [Fact(Skip="This test is not valid for the current code")]
    public void FindExactWord()
    {
        // Arrange
        // var textManager = new TextManager("Hola hola desde xunit");

        // Act
        var result = textManagerGlobal.FindExactWord("mundo", bolIgnoreUppercaseLowercase: true);

        // Assert
        Assert.IsType<List<Match>>(result); //must be of type List<Match>
    }

    [Fact]
    public void FindExactWord_Exception()
    {
        // Arrange
        // var textManager = new TextManager("Hola hola desde xunit");

        // Act
        // Assert
        Assert.ThrowsAny<Exception>(() => textManagerGlobal.FindExactWord(null, bolIgnoreUppercaseLowercase: true)); //Checks if an exception is thrown
    }
}