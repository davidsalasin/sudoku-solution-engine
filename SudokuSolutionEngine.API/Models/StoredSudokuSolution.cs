namespace SudokuSolutionEngine.API.Models;

/// <summary>
/// Model representing a Sudoku solution stored in DynamoDB.
/// </summary>
public class StoredSudokuSolution
{
    /// <summary>
    /// The hash of the board state (Partition Key).
    /// </summary>
    public string BoardHash { get; set; } = string.Empty;

    /// <summary>
    /// Whether the puzzle is solvable.
    /// </summary>
    public bool Solvable { get; set; }

    /// <summary>
    /// The solution board as JSON string (serialized List&lt;List&lt;byte&gt;&gt;).
    /// Contains the solved board if solvable is true, or null if solvable is false.
    /// </summary>
    public string Solution { get; set; } = string.Empty;

    /// <summary>
    /// Unix timestamp when the solution was stored.
    /// </summary>
    public long StoredAt { get; set; }
}
