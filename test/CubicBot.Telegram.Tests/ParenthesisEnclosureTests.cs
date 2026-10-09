using CubicBot.Telegram.Stats;

namespace CubicBot.Telegram.Tests;

public class ParenthesisEnclosureTests
{
    private readonly ParenthesisEnclosure _parenthesisEnclosure = new();

    [Test]
    [Arguments("", false, "")]
    [Arguments("it's nothing", false, "")]
    [Arguments("do(\"str\")", false, "")]
    [Arguments("do(\"str)", true, "\"")]
    [Arguments("()", false, "")]
    [Arguments("(", true, ")")]
    [Arguments(")", true, "(")]
    [Arguments("{([<)]}>", false, "")]
    [Arguments("{([<", true, "})]>")]
    [Arguments("草（", true, "）")]
    public async Task Analyze_Message_Get_Compensation_String(string message, bool expectedResult, string expectedCompensationString)
    {
        var result = _parenthesisEnclosure.AnalyzeMessage(message);
        var compensationString = _parenthesisEnclosure.GetCompensationString();

        await Assert.That(result).IsEqualTo(expectedResult);
        await Assert.That(compensationString).IsEqualTo(expectedCompensationString);
    }
}
