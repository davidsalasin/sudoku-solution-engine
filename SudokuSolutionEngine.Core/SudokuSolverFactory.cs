namespace SudokuSolutionEngine.Core;

/// <summary>
/// Sudoku solver factory implementation.
/// </summary>
public class SudokuSolverFactory : ISudokuSolverFactory
{
    public ISudokuSolver CreateSolver()
    {
        return new DLX();
    }
}