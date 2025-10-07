namespace RobotApp.Logging;

public sealed class PlainConsoleFormatter() : ConsoleFormatter("plain")
{
    public override void Write<TState>(
        in LogEntry<TState> logEntry, 
        IExternalScopeProvider? scopeProvider, 
        TextWriter textWriter
    )
    {
        if (logEntry.Formatter == null) return;

        var message = logEntry.Formatter(logEntry.State, logEntry.Exception);
        if (string.IsNullOrEmpty(message)) return;

        var previous = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = logEntry.LogLevel switch
            {
                LogLevel.Critical => ConsoleColor.Red,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Information => ConsoleColor.Gray,
                LogLevel.Debug => ConsoleColor.DarkGray,
                LogLevel.Trace => ConsoleColor.DarkGray,
                _ => previous
            };

            textWriter.WriteLine(message);
        }
        finally
        {
            Console.ForegroundColor = previous;
        }
    }
}
