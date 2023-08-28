using ValidateInputService.Services;

namespace ValidateInputService.Tests;

public class UnitTest1
{

    private readonly ValidateIdService _validateInput;

    public UnitTest1()
    {
        _validateInput = new ValidateIdService();
    }


    [Fact]
    public void CorrectId()
    {
        string id = "A123456789";
        var result = _validateInput.validateInputIfs(id);
        bool boolResult = result.Any();
        Assert.False(boolResult, "Id written correctly");

    }

    // https://learn.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022
    [Fact]
    public void IncorrectId_LengthLessThanFive()
    {
        string id = "D542";
        var exception = Assert.Throws<ArgumentException>(() => _validateInput.validateInputException(id));
        Assert.Equal("Id must have a length between 5 and 32 characters", exception.Message);
    }
    
    [Fact]
    public void IncorrectId_LengthGreaterThanThirtyTwo()
    {
        string id = "FREGTHY547896321458934789fgtredjuyh";
        var result = _validateInput.validateInputIfs(id);
        bool boolResult = result.Any();
        Assert.True(boolResult, "Greater than 32 in length");
    }

    [Theory]
    [InlineData("fgurhty855")]
    [InlineData("547")]
    public void IncorrectId_NoUpperCaseLetter(string id)
    {
        var result = _validateInput.validateInputRegex(id);
        bool boolResult = result.Any();
        Assert.True(boolResult, "not fulfilling requirements");
    }
}