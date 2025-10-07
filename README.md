# Robot Controller

A simple CLI application that simulates robots moving on a grid. The project is built with .NET 9.0 and uses modern C# features like records, pattern matching, and dependency injection.

## Features

- **Robot simulation**: Robots can move forward (F), rotate left (L) and right (R) on a defined grid
- **Flexible input**: Supports interactive input, stdin piping, and command-line arguments
- **Graceful shutdown**: Supports Ctrl+C to exit the application cleanly
- **Comprehensive logging**: Uses Microsoft.Extensions.Logging with custom console formatter
- **Error handling**: Robust error messages and input validation

## Usage

### Input format
```
<width> <height>    # Room size (optional, updates room size)
<x> <y> <direction> # Robot start position and direction (N,E,S,W)
<commands>          # Sequence of commands: L (left), R (right), F (forward)
```

### Examples

**Multiline input (pipe):**
```bash
echo -e "5 5\n1 2 N\nRFRFFRFRF\n0 0 E\nRFLFFLRF" | dotnet run --project RobotApp
```

**Single-line input (command-line argument):**
```bash
dotnet run --project RobotApp -- "5 5;1 2 N;RFRFFRFRF;0 0 E;RFLFFLRF"
```

**Interactive usage:**
```bash
dotnet run --project RobotApp
```

### Exit codes
- **0**: Success
- **1**: Error during parsing or command execution
- **2**: Cancelled (graceful shutdown, e.g. Ctrl+C)

## Architecture

The project follows clean architecture principles with clear separation of concerns:

### Core Components
- **Models**: `Robot`, `Position`, `Room`, `StartPosition` - Domain logic and state
- **Enums**: `Direction`, `Command` - Type-safe values for directions and commands
- **Services**: Input parsing and reading (`ConsoleInputReader`, `StringInputReader`, `DirectionParser`)
- **Application**: Main application logic with dependency injection and logging

### Technology Stack
- **.NET 9.0** with C# nullable reference types enabled
- **Microsoft.Extensions.DependencyInjection** for IoC
- **Microsoft.Extensions.Logging** with custom console formatter
- **xUnit** for unit testing with comprehensive coverage

### Project Structure
```
RobotApp/
├── Application.cs          # Main application logic
├── Program.cs             # Entry point and DI configuration
├── Models/                # Domain models
├── Services/              # Input handling and parsing
├── Enums/                # Enumerations
├── Interfaces/           # Abstractions
└── Logging/              # Custom logging formatter

RobotApp.Tests/
├── Integration/          # Integration tests
├── Models/              # Model tests
├── Services/            # Service tests
└── Controllers/         # Controller tests
```

## Development

### Run the application
```bash
dotnet run --project RobotApp
```

### Run tests
```bash
dotnet test
```

### Build the project
```bash
dotnet build
```

## CI/CD

The project uses GitHub Actions for continuous integration:

- **Build pipeline**: Automatic build on push/PR to main branch
- **Test execution**: Runs all unit tests with detailed reporting
- **Code coverage**: Collects code coverage with XPlat Code Coverage
- **Codecov integration**: Uploads coverage reports to Codecov
- **Artifacts**: Saves coverage reports as build artifacts

The workflow file is located at `.github/workflows/dotnet.yml` and runs on Ubuntu with .NET 9.0.

## Future Improvements

See [Improvements.md](Improvements.md) for detailed suggestions on future enhancements, including:
- Visualization of robot movement on a map
- Interactive control with arrow keys
- Improved user experience and input handling

