using SudokuSolver.Core;

namespace SudokuSolver.CLI;

static class SudokuPrint {
    /// <summary>
    /// Prints board's matrix with special dynamic GUI according to its puzzle.
    /// </summary>
    public static void Print(this SudokuPuzzle sp)
    {
        // digit amount of the maximum possible value of SudokuPuzzle.
        // (allows to adjust GUI's side accordingly).
        int maxSideDigits = sp.Side.ToString().Length;

        // Custom print GUI method of the board's matrix:

        Console.Write(" ");

        for (var j = 0; j < sp.Side; j++)
        {
            Console.Write("__");

            for (var k = 0; k < maxSideDigits; k++)
            {
                Console.Write("_");
            }

            Console.Write("_");

            if (((j + 1) % Math.Sqrt(sp.Side) == 0) && ((j + 1) != sp.Side))
            {
                Console.Write("__");
            }
        }

        Console.Write("___ \n| ");

        for (var j = 0; j < sp.Side; j++)
        {
            Console.Write(" _");

            for (var k = 0; k < maxSideDigits; k++)
            {
                Console.Write("_");
            }

            Console.Write("_");

            if (((j + 1) % Math.Sqrt(sp.Side) == 0) && ((j + 1) != sp.Side))
            {
                Console.Write("  ");
            }
        }

        Console.Write(" ");

        for (var i = 0; i < sp.Side; i++)
        {
            Console.Write(" |\n| | ");

            for (var j = 0; j < sp.Side; j++)
            {
                for (var k = 0; k < maxSideDigits; k++)
                {
                    Console.Write(" ");
                }

                Console.Write(" | ");

                if (((j + 1) % Math.Sqrt(sp.Side) == 0) && ((j + 1) != sp.Side))
                {
                    Console.Write("| ");
                }
            }

            Console.Write("|\n| | ");

            for (var j = 0; j < sp.Side; j++)
            {
                int indexValue = sp.SudokuMatrix[i, j];

                for (var k = indexValue.ToString().Length; k < maxSideDigits; k++)
                {
                    Console.Write(" ");
                }

                // Print empty space if value is 0, else print value:
                if (sp.SudokuMatrix[i, j] > 0)
                {
                    Console.Write(indexValue);
                }
                else
                {
                    Console.Write(" ");
                }

                Console.Write(" | ");

                if (((j + 1) % Math.Sqrt(sp.Side) == 0) && ((j + 1) != sp.Side))
                {
                    Console.Write("| ");
                }
            }

            Console.Write("|\n| |");

            for (var j = 0; j < sp.Side; j++)
            {
                Console.Write("_");

                for (var k = 0; k < maxSideDigits; k++)
                {
                    Console.Write("_");
                }

                Console.Write("_|");

                if (((j + 1) % Math.Sqrt(sp.Side) == 0) && ((j + 1) != sp.Side))
                {
                    Console.Write(" |");
                }
            }

            if (((i + 1) % Math.Sqrt(sp.Side) == 0) && ((i + 1) != sp.Side))
            {
                Console.Write(" |\n| ");

                for (var j = 0; j < sp.Side; j++)
                {
                    Console.Write(" _");

                    for (var k = 0; k < maxSideDigits; k++)
                    {
                        Console.Write("_");
                    }

                    Console.Write("_");

                    if (((j + 1) % Math.Sqrt(sp.Side) == 0) && ((j + 1) != sp.Side))
                    {
                        Console.Write("  ");
                    }
                }

                Console.Write(" ");
            }
        }

        Console.Write(" |\n|");

        for (var j = 0; j < sp.Side; j++)
        {
            Console.Write("__");

            for (var k = 0; k < maxSideDigits; k++)
            {
                Console.Write("_");
            }

            Console.Write("_");

            if (((j + 1) % Math.Sqrt(sp.Side) == 0) && ((j + 1) != sp.Side))
            {
                Console.Write("__");
            }

        }

        Console.WriteLine("___|\n");
    }
}