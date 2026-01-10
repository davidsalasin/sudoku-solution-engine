# Sudoku Solution Engine

A high-performance Sudoku solver implementation using the DLX (Dancing Links) algorithm, written in C#. Currently provides a command-line interface (CLI) for solving Sudoku puzzles.

## Overview

This project implements an efficient Sudoku solver using Donald Knuth's Algorithm X with the Dancing Links (DLX) technique.

## Features

- Command Line Interface (CLI) executable
- Efficient DLX algorithm implementation for solving Sudoku puzzles

## Quick Start

**For users**: See the [CLI Usage Guide](docs/CLI_USAGE.md) for complete usage instructions and examples.

**For developers**: See the [Development Guide](docs/DEVELOPMENT.md) for building, testing, and contributing.

## Algorithm

This solver uses the **Dancing Links (DLX)** algorithm, which is an efficient implementation of Algorithm X for solving exact cover problems. Sudoku puzzles are converted into exact cover problems, where:

- Each row represents a possible placement of a number in a cell
- Each column represents a constraint that must be satisfied
- The algorithm finds a set of rows that exactly covers all constraints

## Algorithm Constraints

The current implementation has the following constraints:

### Input Format Requirements

- **Square puzzle requirement**: The input puzzle must be square, and its dimensions must follow a specific mathematical relationship. Specifically, the fourth root of the total number of cells (puzzle_length^0.25) must be a natural number. This ensures the puzzle can be divided into equal-sized inner squares (boxes).

  Examples of valid sizes:

  - 9x9 (standard Sudoku): 81 cells, 81^0.25 = 3 ✓
  - 16x16: 256 cells, 256^0.25 = 4 ✓
  - 25x25: 625 cells, 625^0.25 = 5 ✓

### Data Structure Limitations

- **Maximum board size**: The data structure cannot support boards larger than 225x225 (15×15 inner squares). This limitation exists because the implementation uses byte arrays for memory efficiency, requiring values 0-225 (where 0 represents empty cells).

### Solver Algorithm Limitations

- **Practical solving limit**: The DLX solving algorithm currently runs out of memory when attempting to solve Sudoku puzzles larger than 36x36 (6×6 inner squares). This is a temporary limitation that may be addressed in future versions.

  Note: While the data structure can theoretically handle up to 225x225 boards, the solver algorithm is currently limited to 36x36 due to memory constraints during the solving process.

## Developer Documentation

For detailed information on building, testing, and contributing to the project, see the [Development Guide](docs/DEVELOPMENT.md).
