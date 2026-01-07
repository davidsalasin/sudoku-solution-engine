namespace SudokuSolver.Core;

/// <summary>
/// Interface for Sudoku solver.
/// </summary>
public interface ISudokuSolver
{
    /// <summary>
    /// Solves the Sudoku puzzle.
    /// </summary>
    /// <param name="sudoku">The Sudoku puzzle to solve.</param>
    /// <returns>True if the Sudoku puzzle is solved, otherwise false.</returns>
    bool Solve(Sudoku sudoku);
}