# Robot Controller

Simple CLI program that simulates robots moving on a grid.

Usage:
- Provide input via stdin (pipe or file redirection).
- Input format:
  <width> <height>
  <x> <y> <D>
  <commands>

- width/height: integers >= 0 (coordinates inclusive 0..width and 0..height)
- start position: two integers and a direction (N,E,S,W)
- commands: sequence of characters L (left), R (right), F (forward). Commas/spaces are ignored.

- Exit codes:
  - 0: Success.
  - 1: Error while parsing or executing commands.
  - 2: Cancelled (graceful shutdown, e.g. Ctrl+C).

- Graceful shutdown:
  The application supports graceful shutdown via a CancellationToken. When running interactively you can press Ctrl+C to request a shutdown; the app will stop reading further input, log a shutdown message and exit with code 2.

Example:
  echo -e "5 5\n1 2 N\nRFRFFRFRF\n0 0 E\nRFLFFLRF" | dotnet run --project RobotApp/RobotApp.csproj

Architecture:
- Domain: Position, Direction, Room
- Models: Robot (implements IRobot)
- Controllers: RobotController (executes commands)
- Services: Input parsing and console input reader
- Program.cs composes dependencies using Microsoft.Extensions.DependencyInjection and uses ILogger for logging.

## CI / Code coverage
A GitHub Actions workflow (.github/workflows/dotnet.yml) builds and runs tests. Tests collect an XPlat code coverage report which is uploaded as an artifact named "coverage-report" (coverage.cobertura.xml is produced by the test run).

If you use Codecov you can set the CODECOV_TOKEN secret to upload coverage automatically; Codecov can also provide a badge URL to embed in this README.

Badge (placeholder):

![Coverage](https://raw.githubusercontent.com/<OWNER>/<REPO>/main/coverage-badge.svg)

Replace <OWNER> and <REPO> with your repository information, or replace the image URL with the badge URL provided by your coverage provider (Codecov, Coveralls, etc.).
