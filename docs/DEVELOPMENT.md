# Development Guide

Quick reference for developers building, testing, and contributing to the Sudoku Solution Engine.

## Prerequisites

- .NET SDK 10.0 or later
- Code editor (Visual Studio, VS Code, Rider, etc.)

## Project Structure

- **SudokuSolutionEngine.Core** - Core library (DLX algorithm, Sudoku solving logic)
- **SudokuSolutionEngine.CLI** - Command-line interface application
- **SudokuSolutionEngine.Core.Tests** - Unit tests

## Building

```bash
# Clone and restore
git clone <repository-url>
cd sudoku-search-engine
dotnet restore

# Build
dotnet build                    # Debug
dotnet build -c Release         # Release

# Build executable (Windows x64)
scripts/build-cli-win-x64.bat

# Build for other platforms
dotnet publish SudokuSolutionEngine.CLI\SudokuSolutionEngine.CLI.csproj -c Release --self-contained -r <runtime-id> -p:PublishSingleFile=true -p:DebugType=None -o .
# Runtime IDs: linux-x64, osx-x64, osx-arm64
```

## Running

```bash
# Development
dotnet run --project SudokuSolutionEngine.CLI\SudokuSolutionEngine.CLI.csproj

# Executable
.\sudoku-solver.exe
```

## Testing

```bash
dotnet test                                    # Run all tests
dotnet test --logger "console;verbosity=detailed"  # Detailed output
dotnet test --collect:"XPlat Code Coverage"   # With coverage
```

## Architecture

- **Core Layer**: Business logic, DLX algorithm, no external dependencies
- **CLI Layer**: User interaction, input/output handlers
- **Test Layer**: MSTest framework

### Design Patterns

- Factory Pattern: Solvers, input/output handlers
- Facade Pattern: Simplified interface via `Facade` class
- Strategy Pattern: Pluggable input/output handlers

## Contributing

1. Create feature branch from `main`
2. Implement changes with tests
3. Ensure all tests pass
4. Update documentation
5. Submit pull request

### Code Style

- Follow C# conventions
- XML documentation for public APIs
- Single-purpose methods
- Meaningful names
