namespace RobotApp;

public sealed class Application(
    IInputReader input,
    ILogger<Application> logger,
    ServiceProvider serviceProvider
)
{
    /// <summary>
    /// Run the application. Returns 0 on success, 1 on error, 2 on cancellation.
    /// </summary>
    public int Run(CancellationToken cancellationToken)
    {
        ShowInformation();

        if (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Shutdown requested.");
            return 2;
        }

        var room = ReadRoomOrLine();
        if (room == null) return 1;

        if (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Shutdown requested.");
            return 2;
        }

        var start = ReadStartPosition();
        if (start == null) return 1;

        if (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Shutdown requested.");
            return 2;
        }

        var commands = ReadCommands();
        if (commands.Count == 0) return 1;

        var robot = new Robot(serviceProvider.GetRequiredService<ILogger<Robot>>(), start);

        if (!room.Contains(robot.Position))
        {
            logger.LogError("Starting position {X} {Y} is outside the room.", robot.Position.X, robot.Position.Y);
            return 1;
        }

        try
        {
            robot.ExecuteCommand(room, commands);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Execution failed: {Message}", ex.Message);
            return 1;
        }

        logger.LogInformation("Report: {X} {Y} {Direction}", robot.Position.X, robot.Position.Y, robot.Direction);

        return cancellationToken.IsCancellationRequested ? 2 : 0;
    }

    /// <summary>
    /// Shows usage information once when running interactively.
    /// </summary>
    private void ShowInformation()
    {
        if (Console.IsInputRedirected) return;

        logger.LogInformation("Usage:");
        logger.LogInformation("  Provide input via stdin (pipe or file redirection) or interactively via prompts.");
        logger.LogInformation("  Each robot is defined by two lines after an optional room size line:");
        logger.LogInformation("    <width> <height>    -- optional, set or update room size");
        logger.LogInformation("    <x> <y> <D>         -- robot start position and direction (N,E,S,W)");
        logger.LogInformation("    <commands>          -- sequence of characters L (left), R (right), F (forward)");
        logger.LogInformation("");
        logger.LogInformation("Examples:");
        logger.LogInformation("  Multiline (pipe):");
        logger.LogInformation("    echo -e \"5 5\n1 2 N\nRFRFFRFRF\n0 0 E\nRFLFFLRF\" | dotnet run --project RobotApp/RobotApp.csproj");
        logger.LogInformation("");
        logger.LogInformation("  Single-line (run with StringInputReader):");
        logger.LogInformation("    dotnet run --project RobotApp/RobotApp.csproj -- \"5 5;1 2 N;RFRFFRFRF;0 0 E;RFLFFLRF\"");
        logger.LogInformation("");
        logger.LogInformation("Input:");
    }

    private List<Command> ReadCommands()
    {
        var line = input.ReadNonEmptyLine();
        if (line == null)
        {
            logger.LogError("Missing commands for robot.");
            return [];
        }

        var cleaned = Commands.ParseAndClean(line, out var unknownCommands);
        if (!string.IsNullOrEmpty(unknownCommands))
        {
            logger.LogWarning("Ignored unknown command characters: {UnknownCommands}", unknownCommands);
        }

        return cleaned;
    }

    private Room? ReadRoomOrLine()
    {
        var line = input.ReadNonEmptyLine();
        if (line == null)
        {
            logger.LogError("Missing room size input.");
            return null;
        }

        var room = Room.Parse(line);
        if (room != null) return room;

        logger.LogError("Invalid room size. Expected format: '<width> <height>' (e.g. '5 5').");
        return null;
    }

    private StartPosition? ReadStartPosition()
    {
        var line = input.ReadNonEmptyLine();
        if (line == null)
        {
            logger.LogError("Missing robot start input.");
            return null;
        }

        var startPosition = StartPosition.Parse(line);
        if (startPosition != null) return startPosition;

        logger.LogError("Invalid robot start. Expected format: '<x> <y> <direction>' (e.g. '1 2 N').");
        return null;
    }
}
