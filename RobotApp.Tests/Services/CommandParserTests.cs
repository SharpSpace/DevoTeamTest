namespace RobotApp.Tests.Services;

public sealed class CommandParserTests
{
    [Theory]
    [InlineData("L", Command.Left)]
    [InlineData("R", Command.Right)]
    [InlineData("F", Command.Forward)]
    [InlineData("l", Command.Left)]
    [InlineData("r", Command.Right)]
    [InlineData("f", Command.Forward)]
    public void TryParseCommand_Works(string input, Command expected)
    {
        var commands = Commands.ParseAndClean(input, out var unknowns);
        Assert.Equal(expected, commands.Single());
        Assert.Empty(unknowns);
    }

    [Fact]
    public void Clean_ReturnsCommandsAndUnknowns()
    {
        var list = Commands.ParseAndClean("L X R , f", out var unknowns);
        Assert.Equal(3, list.Count);
        Assert.Equal("X", unknowns);
    }
}
