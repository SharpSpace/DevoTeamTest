namespace RobotApp.Models;

public sealed class Robot(ILogger<Robot> logger, StartPosition startPosition)
{
    public Direction Direction { get; private set; } = startPosition.Direction;

    public Position Position { get; private set; } = startPosition.Position;

    public void ExecuteCommand(Room room, List<Command> commands)
    {
        foreach (var cmd in commands)
        {
            Apply(cmd);

            if (room.Contains(Position)) continue;
            var errorMessage = $"Robot moved outside the room at position {Position.X} {Position.Y}.";
            logger.LogWarning(errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }

    private void Apply(Command command)
    {
        switch (command)
        {
            case Command.Forward:
                Position = Direction switch
                {
                    Direction.North => Position with { Y = Position.Y - 1 },
                    Direction.South => Position with { Y = Position.Y + 1 },
                    Direction.East => Position with { X = Position.X + 1 },
                    Direction.West => Position with { X = Position.X - 1 },
                    _ => Position
                };
                break;
            case Command.Left:
                Direction = Direction switch
                {
                    Direction.North => Direction.West,
                    Direction.West => Direction.South,
                    Direction.South => Direction.East,
                    Direction.East => Direction.North,
                    _ => Direction
                };
                break;
            case Command.Right:
                Direction = Direction switch
                {
                    Direction.North => Direction.East,
                    Direction.East => Direction.South,
                    Direction.South => Direction.West,
                    Direction.West => Direction.North,
                    _ => Direction
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(command), command, null);
        }
    }
}
