namespace RobotApp.Tests.Models;

public sealed class RobotModelTests
{
    [Fact]
    public void MoveForward_UpdatesPosition()
    {
        var robot = new Robot(new NullLogger<Robot>(), new StartPosition(new Position(1, 2), Direction.North));
        robot.ExecuteCommand(new Room(1,1), [Command.Forward]);
        Assert.Equal(1, robot.Position.X);
        Assert.Equal(1, robot.Position.Y); // Note: Y decreases when moving North in implementation
    }

    [Fact]
    public void TurnLeft_And_TurnRight_Work()
    {
        var robot = new Robot(new NullLogger<Robot>(), new StartPosition(new Position(0, 0), Direction.North));
        var room = new Room(1, 1);
        robot.ExecuteCommand(room, [Command.Left]);
        Assert.Equal(Direction.West, robot.Direction);
        robot.ExecuteCommand(room, [Command.Right]);
        Assert.Equal(Direction.North, robot.Direction); // back to N
    }
}