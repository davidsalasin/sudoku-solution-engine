# Development Guide

Quick reference for developers building, testing, and contributing to the Sudoku Solution Engine.

## Prerequisites

- .NET SDK 10.0 or later
- **For API with DynamoDB storage**: Docker (for LocalStack) or AWS account (for production)

## Project Structure

- **SudokuSolutionEngine.Core** - Core library (DLX algorithm, Sudoku solving logic)
- **SudokuSolutionEngine.CLI** - Command-line interface application
- **SudokuSolutionEngine.API** - REST API application (see [API Usage Guide](API_USAGE.md))
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
- **UI Layer**: REST API (ASP.NET Core) and CLI interfaces that consume the core library
- **Storage Layer**: Optional DynamoDB storage for caching solutions (API only)
- **Test Layer**: MSTest framework

## Local Development Setup

### Running the API with LocalStack

1. **Start LocalStack**:

   ```bash
   docker-compose -f docker-compose.dev.yml up -d
   ```

2. **Configure the API** (`SudokuSolutionEngine.API/Configuration/appsettings.Development.yaml`):

   ```yaml
   DynamoDB:
     Enabled: true
     ServiceURL: http://localhost:4566
     TableName: SudokuSolutions
     TTL:
       Enabled: true
       Duration: "30.00:00:00"
   ```

3. **Run the API**:

   ```bash
   dotnet run --project SudokuSolutionEngine.API --launch-profile dev
   ```

   The `dev` launch profile automatically sets AWS credentials for LocalStack.

4. **Access DynamoDB Admin UI** (optional):

   - URL: `http://localhost:8001`
   - View tables, items, and manage data

### DynamoDB Table Management

- **Automatic Table Creation**: The API automatically creates the DynamoDB table on startup if it doesn't exist
- **Schema Validation**: Existing tables are validated for correct schema (hash key, TTL configuration)
- **TTL Configuration**: TTL is automatically enabled on the table with the `ttl` attribute
- **LocalStack TTL**: For LocalStack, set `DYNAMODB_REMOVE_EXPIRED_ITEMS=1` in docker-compose to enable TTL cleanup (runs every 60 minutes)
