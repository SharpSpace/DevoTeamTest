namespace RobotApp.Models;

public record StartPosition(Position Position, Direction Direction)
{
    public static StartPosition? Parse(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return null;
        
        var parts = line.Split(
            new[] { ' ', '\t', ',', ';' }, 
            StringSplitOptions.RemoveEmptyEntries
        );
        if (parts.Length < 3) return null;
        if (!int.TryParse(parts[0], out var x) || !int.TryParse(parts[1], out var y)) return null;
        
        var position = new Position(x, y);
        var token = parts[2].Trim();
        
        return !DirectionParser.TryParse(token, out var dir) 
            ? null 
            : new StartPosition(position, dir);
    }
}
