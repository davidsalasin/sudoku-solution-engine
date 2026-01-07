# Sudoku Search Engine

**Currently** only a Sudoku solver implementation using the DLX (Dancing Links) algorithm, written in C#.

## Overview

This project implements an efficient Sudoku solver using Donald Knuth's Algorithm X with the Dancing Links (DLX) technique.

## Features

- Command Line Interface (CLI) application
- Efficient DLX algorithm implementation for solving Sudoku puzzles
- .NET 10.0 compatible
- Self-contained executable build support

## Requirements

- .NET SDK 10.0 or later

## Project Structure

The solution consists of three projects:

- **SudokuSolver.Core** - Core library containing the DLX algorithm and Sudoku solving logic
- **SudokuSolver.CLI** - Command-line interface application
- **SudokuSolver.Core.Tests** - Unit tests for the core library

## Building the Project

1. Clone the repository:

   ```bash
   git clone <repository-url>
   cd sudoku-search-engine
   ```

2. Restore NuGet packages:

   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   dotnet build
   ```

## Building the CLI Executable

To build a self-contained, single-file executable for Windows x64:

```bash
build-cli.bat
```

This will create `SudokuSolver.CLI.exe` in the root directory. The executable is self-contained and does not require .NET to be installed on the target machine.

Alternatively, you can build manually:

```bash
dotnet publish SudokuSolver.CLI\SudokuSolver.CLI.csproj -c Release --self-contained -r win-x64 -p:PublishSingleFile=true -p:DebugType=None -o .
```

## Running the Application

To run the Sudoku solver during development:

```bash
cd SudokuSolver.CLI
dotnet run
```

Or from the solution root:

```bash
dotnet run --project SudokuSolver.CLI\SudokuSolver.CLI.csproj
```

If you've built the executable using `build-cli.bat`, you can run it directly:

```bash
.\SudokuSolver.CLI.exe
```

## Running Tests

To run the unit tests:

```bash
cd SudokuSolver.Core.Tests
dotnet test
```

Or from the solution root:

```bash
dotnet test
```

## Algorithm

This solver uses the **Dancing Links (DLX)** algorithm, which is an efficient implementation of Algorithm X for solving exact cover problems. Sudoku puzzles are converted into exact cover problems, where:

- Each row represents a possible placement of a number in a cell
- Each column represents a constraint that must be satisfied
- The algorithm finds a set of rows that exactly covers all constraints
