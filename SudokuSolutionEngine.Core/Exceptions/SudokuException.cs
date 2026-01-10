namespace SudokuSolutionEngine.Core.Exceptions;

/// <summary>
/// Base exception class for all Sudoku-related validation exceptions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the SudokuException class.
/// </remarks>
/// <param name="message">The error message that explains the reason for the exception.</param>
public abstract class SudokuException(string message) : Exception(message)
{
}
