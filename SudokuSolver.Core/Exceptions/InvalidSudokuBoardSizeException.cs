namespace SudokuSolver.Core.Exceptions;

/// <summary>
/// Exception thrown when the Sudoku puzzle input has an invalid board size.
/// The board size is invalid when (puzzle length)^0.25 is not a natural number.
/// </summary>
/// <remarks>
/// Initializes a new instance of the InvalidSudokuBoardSizeException class.
/// </remarks>
/// <param name="puzzleLength">The length of the puzzle input.</param>
/// <param name="powDot25">The result of (puzzle length)^0.25.</param>
public class InvalidSudokuBoardSizeException(int puzzleLength, double powDot25) : Exception($"Couldn't create Sudoku: (Puzzle length)^0.25 isn't a natural number: {powDot25}")
{

    /// <summary>
    /// Gets the length of the puzzle input that caused the exception.
    /// </summary>
    public int PuzzleLength { get; } = puzzleLength;

    /// <summary>
    /// Gets the result of (puzzle length)^0.25.
    /// </summary>
    public double PowDot25 { get; } = powDot25;
}
