namespace SudokuSolutionEngine.API.Models;

/// <summary>
/// Response model for Sudoku solution operation.
/// </summary>
public class SudokuSolutionResponse
{
    /// <summary>
    /// Indicates whether the Sudoku puzzle was successfully solved.
    /// </summary>
    public bool Solved { get; set; }

    /// <summary>
    /// The solved board as a list of lists of bytes, or null if not solved.
    /// </summary>
    public List<List<byte>>? Board { get; set; }

    /// <summary>
    /// Time taken to solve the puzzle in milliseconds.
    /// </summary>
    public long TimeMs { get; set; }

    /// <summary>
    /// Error message if an error occurred, or null if successful.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Indicates whether this response was retrieved from storage.
    /// </summary>
    public bool RetrievedFromStorage { get; set; }
}
