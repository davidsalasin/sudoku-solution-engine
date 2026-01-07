namespace SudokuSolver.Core;

/// <summary>
/// Interface for Sudoku solver factory.
/// </summary>
public interface ISudokuSolverFactory
{
    /// <summary>
    /// Creates a new Sudoku solver.
    /// </summary>
    /// <returns>A new Sudoku solver.</returns>
    ISudokuSolver CreateSolver();
}