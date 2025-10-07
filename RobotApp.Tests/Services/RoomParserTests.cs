namespace RobotApp.Tests.Services;

public sealed class RoomParserTests
{
    [Fact]
    public void ParseRoomSize_Valid()
    {
        var room = Room.Parse("5 7");
        Assert.NotNull(room);
        Assert.Equal(5, room!.Width);
        Assert.Equal(7, room.Height);
    }
}
