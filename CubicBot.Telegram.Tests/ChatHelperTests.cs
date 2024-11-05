using CubicBot.Telegram.Utils;
using Xunit;

namespace CubicBot.Telegram.Tests;

public class ChatHelperTests
{
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
