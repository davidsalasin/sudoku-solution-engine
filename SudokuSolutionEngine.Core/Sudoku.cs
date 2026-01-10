using SudokuSolutionEngine.Core.Exceptions;

namespace SudokuSolutionEngine.Core;


/// <summary>
/// Class made for a specific Sudoku puzzle.
/// </summary>
public class Sudoku
{
    /// <summary>
    /// Maximum supported inner square size (RootSquareSide).
    /// The maximum supported inner square size is 15, which results in a maximum board size of 225x225 (15*15).
    /// This constant is used to check if the board size exceeds the maximum supported size.
    /// </summary>
    const int MaxRootSquareSide = 15;

    /// <summary>
    /// Maximum supported board side size.
    /// This is MaxRootSquareSide squared (15*15 = 225).
    /// </summary>
    const int MaxBoardSideSize = 225;

    /// <summary>
    /// Practical solver limit for inner square size (RootSquareSide).
    /// The DLX algorithm currently runs out of memory for boards larger than 36x36 (6*6).
    /// This is a temporary limitation that may be addressed in the future.
    /// </summary>
    const int PracticalSolverRootSquareSide = 6;

    /// <summary>
    /// Practical solver limit for board side size.
    /// This is PracticalSolverRootSquareSide squared (6*6 = 36).
    /// </summary>
    const int PracticalSolverBoardSideSize = 36;

    /// <summary>
    /// Board matrix of the Sudoku puzzle.
    /// </summary>
    public byte[,] Board { get; set; }

    /// <summary>
    /// Side length of the puzzle matrix.
    /// </summary>
    public int Side { get; set; }

    /// <summary>
    /// Root square of the side length of the puzzle Matrix. The side length of the inner squares of the puzzle.
    /// </summary>
    public int RootSquareSide { get; set; }

    /// <summary>
    /// Constructs a Sudoku puzzle from a flattened list of bytes.
    /// </summary>
    /// <param name="puzzleInput">The flattened puzzle representation.</param>
    public Sudoku(IList<byte> puzzleInput)
    {
        double powDot25 = Math.Pow(puzzleInput.Count, 0.25);

        if (powDot25 != (int)powDot25)
        {
            throw new InvalidSudokuDimensionsException(puzzleInput.Count, powDot25);
        }

        RootSquareSide = (int)powDot25;
        Side = RootSquareSide * RootSquareSide;

        if (RootSquareSide > MaxRootSquareSide)
        {
            throw new SudokuBoardSizeLimitExceededException(Side, MaxBoardSideSize);
        }

        if (RootSquareSide > PracticalSolverRootSquareSide)
        {
            throw new SudokuSolverLimitExceededException(Side, PracticalSolverBoardSideSize);
        }

        Board = new byte[Side, Side];

        for (var i = 0; i < Side; i++)
        {
            for (var j = 0; j < Side; j++)
            {
                byte indexValue = puzzleInput[i * Side + j];

                if (indexValue > Side)
                {
                    throw new InvalidSudokuValueException(indexValue, Side);
                }

                Board[i, j] = indexValue;
            }
        }
    }

    /// <summary>
    /// Constructs a Sudoku puzzle from a nested list of bytes.
    /// </summary>
    /// <param name="puzzleInput">The nested puzzle representation (rows of bytes).</param>
    public Sudoku(IList<IList<byte>> puzzleInput) : this(puzzleInput.SelectMany(row => row).ToList()) { }
}