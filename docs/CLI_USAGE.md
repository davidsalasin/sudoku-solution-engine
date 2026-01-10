# CLI Usage Guide

Complete guide for using the `sudoku-solver` command-line application.

## Quick Start

```bash
sudoku-solver.exe "530070000600195000098000060800060003400803001700020006060000280000419005000080079"
```

This solves a standard 9×9 Sudoku puzzle and outputs the solution to the console.

## Command-Line Options

### Required Arguments

- **Input** (positional argument `[0]`)
  - The Sudoku puzzle input or file path
  - Required: Yes
  - Format: String of digits (0 = empty cell) or file path

### Optional Arguments

- **`-i, --input-type`**

  - Input source type
  - Values: `Text` (default) | `TextFile`
  - Default: `Text`
  - Description: `Text` reads puzzle from command line, `TextFile` reads from a file

- **`-o, --output-type`**

  - Output destination type
  - Values: `Stdout` (default) | `TextFile`
  - Default: `Stdout`
  - Description: `Stdout` prints to console, `TextFile` writes to a file

- **`-p, --pretty`**

  - Enable pretty printing
  - Type: Flag (boolean)
  - Default: `false`
  - Description: Formats the output in a readable grid format instead of a single line

- **`-l, --logging`**

  - Enable console logging
  - Type: Flag (boolean)
  - Default: `false`
  - Description: Shows detailed logging information during solving

- **Output Path** (positional argument `[1]`)
  - Output file path
  - Required: Only when `--output-type TextFile` is used
  - Description: Path where the solution will be written

## Usage Examples

### Basic Usage

Solve a puzzle from command line and print to console:

```bash
sudoku-solver.exe "530070000600195000098000060800060003400803001700020006060000280000419005000080079"
```

### Pretty Print Output

Display the solution in a formatted grid:

```bash
sudoku-solver.exe -p "530070000600195000098000060800060003400803001700020006060000280000419005000080079"
```

### Read from File

Read puzzle from a file and output to console:

```bash
sudoku-solver.exe -i TextFile puzzle.txt
```

### Save to File

Read puzzle from command line and save solution to a file:

```bash
sudoku-solver.exe -o TextFile "530070000600195000098000060800060003400803001700020006060000280000419005000080079" solution.txt
```

### File to File

Read puzzle from file and save solution to another file:

```bash
sudoku-solver.exe -i TextFile -o TextFile puzzle.txt solution.txt
```

### With Logging

Enable detailed logging during solving:

```bash
sudoku-solver.exe -l "530070000600195000098000060800060003400803001700020006060000280000419005000080079"
```

### Combined Options

Pretty print with logging and save to file:

```bash
sudoku-solver.exe -p -l -o TextFile "530070000600195000098000060800060003400803001700020006060000280000419005000080079" solution.txt
```

## Input Format

### Text Input

The puzzle is represented as a string of digits where:

- `0` represents an empty cell
- `1-9` (or higher for larger puzzles) represent filled cells
- The string length must be a perfect square (e.g., 81 for 9×9, 256 for 16×16)

**Example 9×9 puzzle:**

```
530070000600195000098000060800060003400803001700020006060000280000419005000080079
```

This represents an 81-character string (9×9 = 81 cells).

### TextFile Input

When using `-i TextFile`, provide the file path as the input argument. The file should contain the puzzle string (same format as text input).

**Example file (`puzzle.txt`):**

```
530070000600195000098000060800060003400803001700020006060000280000419005000080079
```

## Output Format

### Standard Output (Default)

By default, the solution is printed as a single line of digits:

```
534678912672195348198342567859761423426853791713924856961537284287419635345286179
```

### Pretty Print (`-p`)

With the `-p` flag, the solution is formatted as a grid.

### File Output (`-o TextFile`)

When using `-o TextFile`, the solution is written to the specified file path. The format (single line or pretty print) depends on whether `-p` is used.

## Puzzle Size Constraints

The solver supports various Sudoku sizes, but with constraints:

- **Input validation**: The puzzle must be square, and `(puzzle_length)^0.25` must be a natural number
  - Valid sizes: 9×9, 16×16, 25×25, 36×36, etc.
- **Maximum board size**: 225×225 (15×15 inner squares)
- **Practical solving limit**: 36×36 (6×6 inner squares) due to memory constraints

See the [README](../README.md#algorithm-constraints) for more details.

## Error Handling

The application will display error messages for:

- Invalid puzzle dimensions
- Invalid cell values
- Missing required arguments
- File I/O errors
- Puzzles exceeding size limits

Example error:

```
Error running Sudoku Solver: Board size 49x49 exceeds the practical solver limit of 36x36...
```

## Getting Help

For more information about the algorithm and constraints, see the [README](../README.md).

For development and building instructions, see the [Development Guide](DEVELOPMENT.md).
