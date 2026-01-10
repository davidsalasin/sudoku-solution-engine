namespace SudokuSolutionEngine.API.Models;

/// <summary>
/// Request model for solving a Sudoku puzzle.
/// </summary>
public class SudokuRequest
{
    /// <summary>
    /// The Sudoku board as a 2D array of bytes.
    /// </summary>
    public List<List<byte>> Board { get; set; } = [];
}
