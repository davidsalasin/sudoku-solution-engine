namespace SudokuSolutionEngine.API.Exceptions;

/// <summary>
/// Base exception class for all API-related exceptions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the SudokuSolutionEngineApiException class.
/// </remarks>
/// <param name="message">The error message that explains the reason for the exception.</param>
public abstract class SudokuSolutionEngineApiException(string message) : Exception(message)
{
}
