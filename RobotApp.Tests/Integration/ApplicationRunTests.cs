using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RobotApp.Tests.Integration;

public sealed class ApplicationRunTests
{
    private ServiceProvider GetServiceProvider()
    {
        // Ensure the custom console formatter is registered
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ILogger<Application>>(_ => new NullLogger<Application>());
        serviceCollection.AddScoped<ILogger<Robot>>(_ => new NullLogger<Robot>());

        return serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public void SingleLineInput_Returns0()
    {
        var input = "5 5;1 2 N;RFRFFRFRF";
        var reader = new StringInputReader(input);
        var app = new Application(reader, new NullLogger<Application>(), GetServiceProvider());

        var exit = app.Run(CancellationToken.None);

        Assert.Equal(0, exit);
    }

    [Fact]
    public void MultiLineInput_Returns0()
    {
        var input = "5 5\n1 2 N\nRFRFFRFRF\n0 0 E\nRFLFFLRF";
        var reader = new StringInputReader(input);
        var app = new Application(reader, new NullLogger<Application>(), GetServiceProvider());

        var exit = app.Run(CancellationToken.None);

        Assert.Equal(0, exit);
    }

    [Fact]
    public void CancelledBeforeStart_Returns2()
    {
        var input = "5 5;1 2 N;RFRFFRFRF";
        var reader = new StringInputReader(input);
        var app = new Application(reader, new NullLogger<Application>(), GetServiceProvider());

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var exit = app.Run(cts.Token);

        Assert.Equal(2, exit);
    }

    [Fact]
    public void ManualInput_Returns0()
    {
        // Simulate manual console input by mocking the ConsoleInputReader behavior
        var lines = new[] { "5 5", "1 2 N", "RFRFFRFRF" };
        var reader = new FakeConsoleInputReader(lines);
        var app = new Application(reader, new NullLogger<Application>(), GetServiceProvider());

        var exit = app.Run(CancellationToken.None);

        Assert.Equal(0, exit);
    }

    private sealed class FakeConsoleInputReader : Interfaces.IInputReader
    {
        private readonly Queue<string> _lines;

        public FakeConsoleInputReader(IEnumerable<string> lines)
        {
            _lines = new Queue<string>(lines.Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim()));
        }

        public string? ReadNonEmptyLine()
        {
            return _lines.Count > 0 ? _lines.Dequeue() : null;
        }
    }
}
