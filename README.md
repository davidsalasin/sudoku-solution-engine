# Sudoku Search Engine

**Currently** only a Sudoku solver implementation using the DLX (Dancing Links) algorithm, written in C#.

## Overview

This project implements an efficient Sudoku solver using Donald Knuth's Algorithm X with the Dancing Links (DLX) technique. The solver can handle standard 9x9 Sudoku puzzles and provides both a console interface and unit tests.

## Project Structure

```
SudokuSearchEngine/
├── SudokuSolver/              # Main application project
│   ├── Program.cs            # Entry point
│   ├── SudokuPuzzle.cs       # Puzzle representation
│   ├── SudokuPannel.cs       # User interface
│   ├── DLX.cs                # Dancing Links algorithm implementation
│   └── MatNodes.cs           # Matrix node structure
└── SudokuSolver.UnitTests/   # Unit test project
    └── PuzzleStringTests.cs  # Test cases
```

## Features

- Efficient DLX algorithm implementation for solving Sudoku puzzles
- Console-based user interface
- Comprehensive unit tests
- .NET 10.0 compatible

## Requirements

- .NET SDK 10.0 or later

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

## Running the Application

To run the Sudoku solver:

```bash
cd SudokuSolver
dotnet run
```

## Running Tests

To run the unit tests:

```bash
cd SudokuSolver.UnitTests
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

