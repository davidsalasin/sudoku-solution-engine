namespace SudokuSolutionEngine.API.Models;

/// <summary>
/// Request model for a Sudoku solution operation.
/// </summary>
public class SudokuSolutionRequest
{
    /// <summary>
    /// The Sudoku board as a 2D array of bytes.
    /// The board is a list of lists of bytes, where each list represents a row of the board.
    /// </summary>
    public List<List<byte>> Board { get; set; } = [];
}
