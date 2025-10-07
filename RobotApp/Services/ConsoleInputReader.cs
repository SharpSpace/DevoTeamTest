namespace RobotApp.Services;

public sealed class ConsoleInputReader : IInputReader
{
    public string? ReadNonEmptyLine()
    {
        while (Console.ReadLine() is { } line)
        {
            if (!string.IsNullOrWhiteSpace(line)) return line.Trim();
        }
        return null;
    }
}
