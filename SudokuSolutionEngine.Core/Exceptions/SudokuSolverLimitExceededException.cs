namespace SudokuSolutionEngine.Core.Exceptions;

/// <summary>
/// Exception thrown when the Sudoku board size exceeds the practical solver limit.
/// The DLX algorithm currently runs out of memory for boards larger than 36x36.
/// This is a temporary limitation that may be addressed in the future.
/// </summary>
/// <remarks>
/// Initializes a new instance of the SudokuSolverLimitExceededException class.
/// </remarks>
/// <param name="boardSize">The board size that exceeded the limit (e.g., 49 for 49x49).</param>
/// <param name="maxSize">The maximum supported board size for solving (36).</param>
public class SudokuSolverLimitExceededException(int boardSize, int maxSize) : Exception($"Board size {boardSize}x{boardSize} exceeds the practical solver limit of {maxSize}x{maxSize}. The DLX algorithm currently runs out of memory for boards larger than this size. This is a temporary limitation.")
{

    /// <summary>
    /// Gets the board size that exceeded the limit.
    /// </summary>
    public int BoardSize { get; } = boardSize;

    /// <summary>
    /// Gets the maximum supported board size for solving.
    /// </summary>
    public int MaxSize { get; } = maxSize;
}
