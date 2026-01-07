using SudokuSolver.Core;

namespace SudokuSolver.CLI.OutputHandlers;

/// <summary>
/// Interface for output handlers.
/// </summary>
public interface IOutputHandler
{
    /// <summary>
    /// Handles the output of a Sudoku puzzle.
    /// </summary>
    /// <param name="sudoku">The Sudoku puzzle to handle.</param>
    void Handle(Sudoku sudoku);
}