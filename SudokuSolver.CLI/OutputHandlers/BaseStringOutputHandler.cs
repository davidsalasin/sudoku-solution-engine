using System.Text;
using Microsoft.Extensions.Logging;
using SudokuSolver.Core;

namespace SudokuSolver.CLI.OutputHandlers;

/// <summary>
/// Base class for output handlers that handle output from a string.
/// </summary>
public abstract class BaseStringOutputHandler(ILogger logger, bool prettyPrint) : IOutputHandler
{
    /// <summary>
    /// Handles the output of a Sudoku puzzle.
    /// </summary>
    /// <param name="sudoku">The Sudoku puzzle to handle.</param>
    public abstract void Handle(Sudoku sudoku);

    /// <summary>
    /// Converts a Sudoku puzzle to a string.
    /// </summary>
    /// <param name="sudoku">The Sudoku puzzle to convert.</param>
    /// <returns>A string representation of the Sudoku puzzle.</returns>
    protected string SudokuToString(Sudoku sudoku)
    {
        if (prettyPrint)
        {
            logger.LogInformation("Pretty printing Sudoku puzzle");
            return SudokuToPrettyString(sudoku);
        }
        logger.LogInformation("Plain printing Sudoku puzzle");
        return SudokuToPlainString(sudoku);
    }


    /// <summary>
    /// Converts a Sudoku puzzle to a pretty string.
    /// </summary>
    /// <param name="sudoku">The Sudoku puzzle to convert.</param>
    /// <returns>A pretty string representation of the Sudoku puzzle.</returns>
    private string SudokuToPrettyString(Sudoku sudoku)
    {
        // digit amount of the maximum possible value of SudokuPuzzle.
        // (allows to adjust GUI's side accordingly).
        int maxSideDigits = sudoku.Side.ToString().Length;

        var sb = new StringBuilder();

        sb.Append(" ");

        for (var j = 0; j < sudoku.Side; j++)
        {
            sb.Append("__");

            for (var k = 0; k < maxSideDigits; k++)
            {
                sb.Append("_");
            }

            sb.Append("_");

            if (((j + 1) % Math.Sqrt(sudoku.Side) == 0) && ((j + 1) != sudoku.Side))
            {
                sb.Append("__");
            }
        }

        sb.Append("___ \n| ");

        for (var j = 0; j < sudoku.Side; j++)
        {
            sb.Append(" _");

            for (var k = 0; k < maxSideDigits; k++)
            {
                sb.Append("_");
            }

            sb.Append("_");

            if (((j + 1) % Math.Sqrt(sudoku.Side) == 0) && ((j + 1) != sudoku.Side))
            {
                sb.Append("  ");
            }
        }

        sb.Append(" ");

        for (var i = 0; i < sudoku.Side; i++)
        {
            sb.Append(" |\n| | ");

            for (var j = 0; j < sudoku.Side; j++)
            {
                for (var k = 0; k < maxSideDigits; k++)
                {
                    sb.Append(" ");
                }

                sb.Append(" | ");

                if (((j + 1) % Math.Sqrt(sudoku.Side) == 0) && ((j + 1) != sudoku.Side))
                {
                    sb.Append("| ");
                }
            }

            sb.Append("|\n| | ");

            for (var j = 0; j < sudoku.Side; j++)
            {
                byte cellValue = sudoku.Board[i, j];
                int indexValue = cellValue;

                for (var k = indexValue.ToString().Length; k < maxSideDigits; k++)
                {
                    sb.Append(" ");
                }

                // Print empty space if value is 0, else print value:
                if (cellValue > 0)
                {
                    sb.Append(indexValue);
                }
                else
                {
                    sb.Append(" ");
                }

                sb.Append(" | ");

                if (((j + 1) % Math.Sqrt(sudoku.Side) == 0) && ((j + 1) != sudoku.Side))
                {
                    sb.Append("| ");
                }
            }

            sb.Append("|\n| |");

            for (var j = 0; j < sudoku.Side; j++)
            {
                sb.Append("_");

                for (var k = 0; k < maxSideDigits; k++)
                {
                    sb.Append("_");
                }

                sb.Append("_|");

                if (((j + 1) % Math.Sqrt(sudoku.Side) == 0) && ((j + 1) != sudoku.Side))
                {
                    sb.Append(" |");
                }
            }

            if (((i + 1) % Math.Sqrt(sudoku.Side) == 0) && ((i + 1) != sudoku.Side))
            {
                sb.Append(" |\n| ");

                for (var j = 0; j < sudoku.Side; j++)
                {
                    sb.Append(" _");

                    for (var k = 0; k < maxSideDigits; k++)
                    {
                        sb.Append("_");
                    }

                    sb.Append("_");

                    if (((j + 1) % Math.Sqrt(sudoku.Side) == 0) && ((j + 1) != sudoku.Side))
                    {
                        sb.Append("  ");
                    }
                }

                sb.Append(" ");
            }
        }

        sb.Append(" |\n|");

        for (var j = 0; j < sudoku.Side; j++)
        {
            sb.Append("__");

            for (var k = 0; k < maxSideDigits; k++)
            {
                sb.Append("_");
            }

            sb.Append("_");

            if (((j + 1) % Math.Sqrt(sudoku.Side) == 0) && ((j + 1) != sudoku.Side))
            {
                sb.Append("__");
            }

        }

        sb.AppendLine("___|");
        sb.AppendLine();

        return sb.ToString();
    }


    /// <summary>
    /// Converts a Sudoku puzzle to a plain string.
    /// </summary>
    /// <param name="sudoku">The Sudoku puzzle to convert.</param>
    /// <returns>A plain string representation of the Sudoku puzzle.</returns>
    private string SudokuToPlainString(Sudoku sudoku)
    {
        return string.Join("", sudoku.Board.Cast<byte>().Select(x => x.ToString()));
    }
}