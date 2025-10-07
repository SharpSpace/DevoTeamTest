namespace RobotApp;

public static class Commands
{
    private static readonly char[] allowedChars = Enum.GetNames(typeof(Command))
        .Where(n => !string.IsNullOrEmpty(n))
        .SelectMany(n => new[] { char.ToUpperInvariant(n[0]), char.ToLowerInvariant(n[0]) })
        .Distinct()
        .ToArray();

    public static List<Command> ParseAndClean(string input, out string unknownCommands)
    {
        var commands = new List<Command>();
        var unknowns = new System.Text.StringBuilder();
        foreach (var ch in input.Where(ch => !char.IsWhiteSpace(ch) && ch != ',' && ch != ';'))
        {
            if (!IsAllowed(ch))
            {
                unknowns.Append(ch);
                continue;
            }

            if (TryParse(ch, out var cmd))
            {
                commands.Add(cmd);
            }
            else
            {
                unknowns.Append(ch);
            }
        }

        unknownCommands = unknowns.Length > 0 ? unknowns.ToString() : string.Empty;
        return commands;
    }

    private static bool IsAllowed(char ch) => Array.IndexOf(allowedChars, ch) >= 0;

    private static bool TryParse(char ch, out Command cmd)
    {
        cmd = Command.Forward;
        switch (char.ToUpperInvariant(ch))
        {
            case 'L':
                cmd = Command.Left;
                return true;
            case 'R':
                cmd = Command.Right;
                return true;
            case 'F':
                cmd = Command.Forward;
                return true;
            default:
                return false;
        }
    }
}
