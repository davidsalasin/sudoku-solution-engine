namespace SudokuSolver;


//Class SudokuPuzzle: class made for a specific Sudoku puzzle
public class SudokuPuzzle
{
    // Property: string SudokuSTR.
    // Puzzle string of the Sudoku puzzle.
    public string SudokuSTR { get; set; }
    // Property: int[,] SudokuMatrix.
    // Board matrix of the Sudoku puzzle.
    public int[,] SudokuMatrix { get; set; }
    // Property: int Side.
    // Side lenght of the puzzle matrix.
    public int Side { get; set; }
    // Property: int RootSquareSide.
    // Root square of the side lenght of the puzzle Matrix -> The side lenght of the inner squares of the puzzle.
    public int RootSquareSide { get; set; }

    // Class Constructor.
    // RECEIVES: string puzzleString.
    // CONSTRUCTS -> SudokuPuzzle with received puzzleString, Board Matrix according to puzzle's values, Side lenght
    //               of the board's values and Inner Side lenght of the inner boards of the Sudoku puzzle.
    public SudokuPuzzle(string puzzleString)
    {
        // The next call of the pow method allows the constuctor to check if the lenght of the received puzzle string 
        // is valid to create a board of sudoku, by checking if its lenght powered 0.25 is a natural number.
        double powDot25 = Math.Pow(puzzleString.Length, 0.25);

        // if powDot25 a natural number -> stop constructing the class by generating a custom texted exception.
        // The texted exception -> notifies user about inputed puzzle string not being fitting sudoku's standarts.
        if (powDot25 != (int)powDot25)
        {
            throw new Exception($"Couldn't create SudokuPuzzle Class: (Puzzle string's lenght)^0.25 isn't a natural number: {powDot25}");
        }

        // Assigns values to all self properties:
        SudokuSTR = puzzleString;
        RootSquareSide = (int)powDot25;
        Side = RootSquareSide * RootSquareSide;
        SudokuMatrix = new int[Side, Side];

        // While moving on board's matrix:
        for (var i = 0; i < Side; i++)
        {
            for (var j = 0; j < Side; j++)
            {
                // value of puzzle string's current index
                var indexValue = (int)puzzleString[i * Side + j] - '0';

                // if indexValue isn't in range of the accepts value for the specific puzzle -> stop constructing the class by generating a custom texted exception.
                // The texted exception -> notifies user about inputed puzzle string containing unfitting char values for the specific puzzle.
                if (indexValue < 0 || indexValue > Side)
                {
                    throw new Exception("Couldn't create SudokuPuzzle Class: Puzzle string contains invalid charactrs for the Lenght of the Soduko puzzle string: "
                        + $"'{puzzleString[i * Side + j]}' (as {indexValue})");
                }

                // assign value of puzzle string's current index to board's matrix aligned index.
                SudokuMatrix[i, j] = indexValue;
            }
        }
    }

    // Method: Print.
    // RETURNS: void ** Prints board's matrix with special dynamic GUI according to its puzzle **.
    public void Print()
    {
        // digit amount of the maximum possible value of SudokuPuzzle.
        // (allows to adjust GUI's side accordingly).
        int maxSideDigits = Side.ToString().Length;

        // Custom print GUI method of the board's matrix:

        Console.Write(" ");

        for (var j = 0; j < Side; j++)
        {
            Console.Write("__");

            for (var k = 0; k < maxSideDigits; k++)
            {
                Console.Write("_");
            }

            Console.Write("_");

            if (((j + 1) % Math.Sqrt(Side) == 0) && ((j + 1) != Side))
            {
                Console.Write("__");
            }
        }

        Console.Write("___ \n| ");

        for (var j = 0; j < Side; j++)
        {
            Console.Write(" _");

            for (var k = 0; k < maxSideDigits; k++)
            {
                Console.Write("_");
            }

            Console.Write("_");

            if (((j + 1) % Math.Sqrt(Side) == 0) && ((j + 1) != Side))
            {
                Console.Write("  ");
            }
        }

        Console.Write(" ");

        for (var i = 0; i < Side; i++)
        {
            Console.Write(" |\n| | ");

            for (var j = 0; j < Side; j++)
            {
                for (var k = 0; k < maxSideDigits; k++)
                {
                    Console.Write(" ");
                }

                Console.Write(" | ");

                if (((j + 1) % Math.Sqrt(Side) == 0) && ((j + 1) != Side))
                {
                    Console.Write("| ");
                }
            }

            Console.Write("|\n| | ");

            for (var j = 0; j < Side; j++)
            {
                int indexValue = SudokuMatrix[i, j];

                for (var k = indexValue.ToString().Length; k < maxSideDigits; k++)
                {
                    Console.Write(" ");
                }

                // Print empty space if value is 0, else print value:
                if (SudokuMatrix[i, j] > 0)
                {
                    Console.Write(indexValue);
                }
                else
                {
                    Console.Write(" ");
                }

                Console.Write(" | ");

                if (((j + 1) % Math.Sqrt(Side) == 0) && ((j + 1) != Side))
                {
                    Console.Write("| ");
                }
            }

            Console.Write("|\n| |");

            for (var j = 0; j < Side; j++)
            {
                Console.Write("_");

                for (var k = 0; k < maxSideDigits; k++)
                {
                    Console.Write("_");
                }

                Console.Write("_|");

                if (((j + 1) % Math.Sqrt(Side) == 0) && ((j + 1) != Side))
                {
                    Console.Write(" |");
                }
            }

            if (((i + 1) % Math.Sqrt(Side) == 0) && ((i + 1) != Side))
            {
                Console.Write(" |\n| ");

                for (var j = 0; j < Side; j++)
                {
                    Console.Write(" _");

                    for (var k = 0; k < maxSideDigits; k++)
                    {
                        Console.Write("_");
                    }

                    Console.Write("_");

                    if (((j + 1) % Math.Sqrt(Side) == 0) && ((j + 1) != Side))
                    {
                        Console.Write("  ");
                    }
                }

                Console.Write(" ");
            }
        }

        Console.Write(" |\n|");

        for (var j = 0; j < Side; j++)
        {
            Console.Write("__");

            for (var k = 0; k < maxSideDigits; k++)
            {
                Console.Write("_");
            }

            Console.Write("_");

            if (((j + 1) % Math.Sqrt(Side) == 0) && ((j + 1) != Side))
            {
                Console.Write("__");
            }

        }

        Console.WriteLine("___|\n");
    }

    // Method: Solve.
    // RETURNS: true if managed to solve the puzzle, else false.
    public bool Solve()
    {
        // Calls algorithm DLX (Solve as a static class method) to solve the given SudokuPuzzle(self).
        // ** Updates Board accordingly inside of the static method, if solved **.
        return DLX.Solve(this); ;
    }
}

