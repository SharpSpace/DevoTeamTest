namespace RobotApp.Models;

public sealed class Room
{
    public Room(int width, int height)
    {
        if (width < 0 || height < 0) throw new ArgumentException("Room dimensions must be non-negative");
        Width = width;
        Height = height;
    }

    public int Height { get; }

    public int Width { get; }

    public static Room? Parse(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return null;
        var parts = line.Split(new[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2) return null;
        if (!int.TryParse(parts[0], out var width)) return null;
        if (!int.TryParse(parts[1], out var height)) return null;
        if (width < 0 || height < 0) return null;
        return new Room(width, height);
    }

    public bool Contains(Position p) => p.X >= 0 && p.X <= Width && p.Y >= 0 && p.Y <= Height;
}
