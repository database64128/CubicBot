using CubicBot.Telegram.Stats;
using Xunit;

namespace CubicBot.Telegram.Tests;

public class ParenthesisEnclosureTests
{
    private readonly ParenthesisEnclosure _parenthesisEnclosure = new();

    [Theory]
    [InlineData("", false, "")]
    [InlineData("it's nothing", false, "")]
    [InlineData("do(\"str\")", false, "")]
    [InlineData("do(\"str)", true, "\"")]
    [InlineData("()", false, "")]
    [InlineData("(", true, ")")]
    [InlineData(")", true, "(")]
    [InlineData("{([<)]}>", false, "")]
    [InlineData("{([<", true, "})]>")]
    [InlineData("草（", true, "）")]
    public void Analyze_Message_Get_Compensation_String(string message, bool expectedResult, string expectedCompensationString)
    {
        var result = _parenthesisEnclosure.AnalyzeMessage(message);
        var compensationString = _parenthesisEnclosure.GetCompensationString();

        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedCompensationString, compensationString);
    }
}
