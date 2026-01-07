namespace SudokuSolver.Core.Exceptions;

/// <summary>
/// Exception thrown when the Sudoku board size exceeds the maximum supported size.
/// The maximum supported size is 15x15 due to memory efficiency constraints (using byte arrays).
/// </summary>
/// <remarks>
/// Initializes a new instance of the SudokuBoardSizeLimitExceededException class.
/// </remarks>
/// <param name="boardSize">The board size that exceeded the limit (e.g., 16 for 16x16).</param>
/// <param name="maxSize">The maximum supported board size (15).</param>
public class SudokuBoardSizeLimitExceededException(int boardSize, int maxSize) : Exception($"Board size {boardSize}x{boardSize} exceeds maximum supported size of {maxSize}x{maxSize}. This limitation exists because the implementation uses byte arrays for memory efficiency, requiring values 0-{maxSize} (where 0 represents empty cells).")
{

    /// <summary>
    /// Gets the board size that exceeded the limit.
    /// </summary>
    public int BoardSize { get; } = boardSize;

    /// <summary>
    /// Gets the maximum supported board size.
    /// </summary>
    public int MaxSize { get; } = maxSize;
}
