namespace RobotApp.Services;

public static class DirectionParser
{
    public static bool TryParse(string? token, out Direction dir)
    {
        dir = Direction.North;
        if (string.IsNullOrWhiteSpace(token)) return false;

        token = token.Trim();

        if (token.Length != 1) return false;
        
        switch (char.ToUpperInvariant(token[0]))
        {
            case 'N':
                dir = Direction.North;
                return true;
            case 'E':
                dir = Direction.East;
                return true;
            case 'S':
                dir = Direction.South;
                return true;
            case 'W':
                dir = Direction.West;
                return true;
            default:
                return false;
        }
    }
}
