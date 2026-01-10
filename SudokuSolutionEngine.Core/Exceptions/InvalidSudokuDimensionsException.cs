namespace SudokuSolutionEngine.Core.Exceptions;

/// <summary>
/// Exception thrown when the Sudoku puzzle input has invalid dimensions that don't form a valid square sudoku.
/// The dimensions are invalid when (puzzle length)^0.25 is not a natural number.
/// </summary>
/// <remarks>
/// Initializes a new instance of the InvalidSudokuDimensionsException class.
/// </remarks>
/// <param name="puzzleLength">The length of the puzzle input.</param>
/// <param name="powDot25">The result of (puzzle length)^0.25.</param>
public class InvalidSudokuDimensionsException(int puzzleLength, double powDot25) : SudokuException($"Couldn't create Sudoku: (Puzzle length)^0.25 isn't a natural number: {powDot25:F3}...")
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
