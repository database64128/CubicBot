using CubicBot.Telegram.Utils;
using Telegram.Bot.Exceptions;
using Xunit;

namespace CubicBot.Telegram.Tests;

public class ChatHelperTests
{
    [Fact]
    public void Get_Retry_Wait_Time_Normal()
    {
        for (var i = 1; i <= 30; i++)
        {
            Test_Get_Retry_Wait_Time(i);
        }
    }

    private static void Test_Get_Retry_Wait_Time(int seconds)
    {
        var ex = new ApiRequestException($"Too Many Requests: retry after {seconds}", 429);

        var waitTimeSec = ChatHelper.GetRetryWaitTimeMs(ex) / 1000;

        Assert.InRange(waitTimeSec, seconds + 1, seconds + 5);
    }

    [Theory]
    [InlineData("💩")]
    [InlineData("Super super super super super super super long error message.")]
    public void Get_Retry_Wait_Time_Bad_Message(string message)
    {
        var ex = new ApiRequestException(message, 429);

        var waitTimeMs = ChatHelper.GetRetryWaitTimeMs(ex);

        Assert.Equal(15 * 1000, waitTimeMs);
    }

    [Theory]
    [InlineData(null, "fakename", null, null)]
    [InlineData("lol", "fakename", null, null)]
    [InlineData("/", "fakename", null, null)]
    [InlineData("/ arg", "fakename", null, null)]
    [InlineData("/@", "fakename", null, null)]
    [InlineData("/@ arg", "fakename", null, null)]
    [InlineData("/@fakename", "fakename", null, null)]
    [InlineData("/@fakename arg", "fakename", null, null)]
    [InlineData("/@wrongname", "fakename", null, null)]
    [InlineData("/start", "fakename", "start", null)]
    [InlineData("/start ", "fakename", "start", null)]
    [InlineData("/start  ", "fakename", "start", null)]
    [InlineData("/start arg", "fakename", "start", "arg")]
    [InlineData("/start arg ", "fakename", "start", "arg")]
    [InlineData("/start  arg", "fakename", "start", "arg")]
    [InlineData("/start@ arg", "fakename", "start", "arg")]
    [InlineData("/start@wrongname arg", "fakename", null, null)]
    [InlineData("/start@fakename arg", "fakename", "start", "arg")]
    public void Parse_Message_Into_Command_And_Argument(string? message, string botUsername, string? expectedCommand, string? expectedArgument)
    {
        var (command, argument) = ChatHelper.ParseMessageIntoCommandAndArgument(message, botUsername);

        Assert.Equal(expectedCommand, command);
        Assert.Equal(expectedArgument, argument);
    }
}
