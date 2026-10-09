using CubicBot.Telegram.Utils;

namespace CubicBot.Telegram.Tests;

public class ChatHelperTests
{
    [Test]
    [Arguments(null, "fakename", null, null)]
    [Arguments("lol", "fakename", null, null)]
    [Arguments("/", "fakename", null, null)]
    [Arguments("/ arg", "fakename", null, null)]
    [Arguments("/@", "fakename", null, null)]
    [Arguments("/@ arg", "fakename", null, null)]
    [Arguments("/@fakename", "fakename", null, null)]
    [Arguments("/@fakename arg", "fakename", null, null)]
    [Arguments("/@wrongname", "fakename", null, null)]
    [Arguments("/start", "fakename", "start", null)]
    [Arguments("/start ", "fakename", "start", null)]
    [Arguments("/start  ", "fakename", "start", null)]
    [Arguments("/start arg", "fakename", "start", "arg")]
    [Arguments("/start arg ", "fakename", "start", "arg")]
    [Arguments("/start  arg", "fakename", "start", "arg")]
    [Arguments("/start@ arg", "fakename", "start", "arg")]
    [Arguments("/start@wrongname arg", "fakename", null, null)]
    [Arguments("/start@fakename arg", "fakename", "start", "arg")]
    public async Task Parse_Message_Into_Command_And_Argument(string? message, string botUsername, string? expectedCommand, string? expectedArgument)
    {
        var (command, argument) = ChatHelper.ParseMessageIntoCommandAndArgument(message, botUsername);

        await Assert.That(command).IsEqualTo(expectedCommand);
        await Assert.That(argument).IsEqualTo(expectedArgument);
    }
}
