namespace SudokuSolver.Core.Exceptions;

/// <summary>
/// Exception thrown when the Sudoku puzzle input contains an invalid value.
/// A value is invalid when it is less than 0 or greater than the side length of the puzzle.
/// </summary>
/// <remarks>
/// Initializes a new instance of the InvalidSudokuValueException class.
/// </remarks>
/// <param name="invalidValue">The invalid value that was found in the puzzle input.</param>
/// <param name="side">The side length of the Sudoku puzzle.</param>
public class InvalidSudokuValueException(int invalidValue, int side) : Exception($"Couldn't create Sudoku: Invalid value '{invalidValue}' found in puzzle input for side length '{side}'")
{

    /// <summary>
    /// Gets the invalid value that caused the exception.
    /// </summary>
    public int InvalidValue { get; } = invalidValue;

    /// <summary>
    /// Gets the side length of the Sudoku puzzle.
    /// </summary>
    public int Side { get; } = side;
}
