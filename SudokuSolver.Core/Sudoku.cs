namespace SudokuSolver.Core;


/// <summary>
/// Class made for a specific Sudoku puzzle.
/// </summary>
public class Sudoku
{

    /// <summary>
    /// Board matrix of the Sudoku puzzle.
    /// </summary>
    public int[,] Board { get; set; }

    /// <summary>
    /// Side length of the puzzle matrix.
    /// </summary>
    public int Side { get; set; }

    /// <summary>
    /// Root square of the side length of the puzzle Matrix. The side length of the inner squares of the puzzle.
    /// </summary>
    public int RootSquareSide { get; set; }

    /// <summary>
    /// Constructs a SudokuPuzzle with the received puzzleString, Board Matrix according to puzzle's values,
    /// Side length of the board's values and Inner Side length of the inner boards of the Sudoku puzzle.
    /// </summary>
    /// <param name="puzzleString">The puzzle string representation of the Sudoku puzzle.</param>
    public Sudoku(IList<int> puzzleInput)
    {
        // The next call of the pow method allows the constructor to check if the length of the received puzzle string
        // is valid to create a board of sudoku, by checking if its length powered 0.25 is a natural number.
        double powDot25 = Math.Pow(puzzleInput.Count, 0.25);

        // If powDot25 is not a natural number, stop constructing the class by generating a custom exception.
        // The exception notifies user about inputted puzzle string not being fitting sudoku's standards.
        if (powDot25 != (int)powDot25)
        {
            throw new Exception($"Couldn't create Sudoku: (Puzzle string's lenght)^0.25 isn't a natural number: {powDot25}");
        }

        RootSquareSide = (int)powDot25;
        Side = RootSquareSide * RootSquareSide;
        Board = new int[Side, Side];

        // While moving on board's matrix:
        for (var i = 0; i < Side; i++)
        {
            for (var j = 0; j < Side; j++)
            {
                // value of puzzle string's current index
                var indexValue = puzzleInput[i * Side + j];

                // If indexValue isn't in range of the accepted value for the specific puzzle, stop constructing the class by generating a custom exception.
                // The exception notifies user about inputted puzzle string containing unfitting char values for the specific puzzle.
                if (indexValue < 0 || indexValue > Side)
                {
                    throw new Exception("Couldn't create Sudoku: Puzzle string contains invalid charactrs for the Lenght of the Sudoku puzzle string: "
                        + $"'{indexValue}'");
                }

                // assign value of puzzle string's current index to board's matrix aligned index.
                Board[i, j] = indexValue;
            }
        }
    }
}

