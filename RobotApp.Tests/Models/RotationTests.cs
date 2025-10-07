namespace RobotApp.Tests.Models;

public sealed class RotationTests
{
    [Theory]
    [InlineData(Direction.North, Command.Left, Direction.West)]
    [InlineData(Direction.North, Command.Right, Direction.East)]
    [InlineData(Direction.South, Command.Left, Direction.East)]
    [InlineData(Direction.South, Command.Right, Direction.West)]
    public void Rotation_Works(Direction start, Command cmd, Direction expected)
    {
        var robot = new Robot(
            new NullLogger<Robot>(), 
            new StartPosition(new Position(1, 1), start)
        );
        robot.ExecuteCommand(new Room(1,1), [cmd]);
        Assert.Equal(expected, robot.Direction);
    }
}
