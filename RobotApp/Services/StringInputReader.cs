namespace RobotApp.Services;

public class StringInputReader : IInputReader
{
    private readonly Queue<string> _lines;

    public StringInputReader(string[] args)
    {
        var joined = string.Join(' ', args);
        _lines = CreateQueueFromInput(joined);
    }

    public StringInputReader(string input) => _lines = CreateQueueFromInput(input);

    private static Queue<string> CreateQueueFromInput(string input)
    {
        var normalized = input.Replace("\r\n", "\n").Replace('\r', '\n');
        var parts = normalized
            .Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p));
        return new Queue<string>(parts);
    }

    public string? ReadNonEmptyLine() => _lines.Count > 0 ? _lines.Dequeue() : null;
}
