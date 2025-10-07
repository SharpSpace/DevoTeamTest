namespace RobotApp.Tests.Controllers;

public sealed class RobotServiceTests
{
    [Fact]
    public void RobotMovesForwardWithinBounds()
    {
        var room = new Room(5, 5);
        var robot = new Robot(new NullLogger<Robot>(), new StartPosition(new Position(1, 1), Direction.North));

        var commands = Commands.ParseAndClean("F", out var unknowns);
        Assert.Empty(unknowns);

        robot.ExecuteCommand(room, commands);

        Assert.Equal(new Position(1, 0), robot.Position);
    }

    [Fact]
    public void RobotLeavesBounds_Throws()
    {
        var room = new Room(1, 1);
        var robot = new Robot(new NullLogger<Robot>(), new StartPosition(new Position(0, 0), Direction.West));

        var commands = Commands.ParseAndClean("F", out var unknowns);
        Assert.Empty(unknowns);

        var exception = Assert.Throws<InvalidOperationException>(() => 
            robot.ExecuteCommand(room, commands)
        );
        Assert.NotNull(exception.Message);
    }

    [Fact]
    public void RobotMovesOutOfBoundsSouth()
    {
        var room = new Room(2, 2);
        var robot = new Robot(new NullLogger<Robot>(), new StartPosition(new Position(0, 0), Direction.South));

        // first two moves within bounds
        var commands = Commands.ParseAndClean("FFF", out var unknowns);
        Assert.Empty(unknowns);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            robot.ExecuteCommand(room, commands)
        );
        Assert.Equal("Robot moved outside the room at position 0 3.", exception.Message);
    }

    [Fact]
    public void RobotMovesOutOfBoundsWest()
    {
        var room = new Room(2, 2);
        var robot = new Robot(new NullLogger<Robot>(), new StartPosition(new Position(0, 0), Direction.East));

        var commands = Commands.ParseAndClean("FFF", out var unknowns);
        Assert.Empty(unknowns);

        var ex = Assert.Throws<InvalidOperationException>(() => 
            robot.ExecuteCommand(room, commands)
        );
        Assert.Equal("Robot moved outside the room at position 3 0.", ex.Message);
    }

    [Fact]
    public void RobotMovesOutOfBoundsNorth()
    {
        var room = new Room(2, 2);
        var robot = new Robot(new NullLogger<Robot>(), new StartPosition(new Position(0, 2), Direction.North));

        var commands = Commands.ParseAndClean("FFF", out var unknowns);
        Assert.Empty(unknowns);

        var ex = Assert.Throws<InvalidOperationException>(() => 
            robot.ExecuteCommand(room, commands)
        );
        Assert.Equal("Robot moved outside the room at position 0 -1.", ex.Message);
    }
}
