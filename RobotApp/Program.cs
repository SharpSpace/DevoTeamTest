var services = new ServiceCollection();

services.AddLogging(builder =>
{
    builder.ClearProviders();
    // register the formatter implementation so AddConsole can find it
    builder.Services.AddSingleton<ConsoleFormatter, PlainConsoleFormatter>();
    builder.AddConsole(options => { options.FormatterName = "plain"; });
});

// If a command-line argument is provided, treat it as single-line input and use StringInputReader.
if (args is { Length: > 0 })
{
    services.AddSingleton<IInputReader>(_ => new StringInputReader(args));
}
else
{
    services.AddSingleton<IInputReader, ConsoleInputReader>();
}

services.AddSingleton<Application>();

using var provider = services.BuildServiceProvider();

var app = provider.GetRequiredService<Application>();

using var cancellationTokenSource = new CancellationTokenSource();
// Cancel on Ctrl+C
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true; // prevent process termination so we can shut down gracefully
    cancellationTokenSource.Cancel();
};

var exit = app.Run(cancellationTokenSource.Token);
Environment.Exit(exit);
