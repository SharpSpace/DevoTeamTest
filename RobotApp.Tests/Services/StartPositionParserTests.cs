namespace RobotApp.Tests.Services;

public sealed class StartPositionParserTests
{
    [Fact]
    public void ParseStart_Valid()
    {
        var start = StartPosition.Parse("1 2 N");
        Assert.NotNull(start);
        Assert.Equal(1, start!.Position.X);
        Assert.Equal(2, start.Position.Y);
        Assert.Equal(Direction.North, start.Direction);
    }
}
