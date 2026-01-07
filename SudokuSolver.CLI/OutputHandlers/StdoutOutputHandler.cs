using SudokuSolver.Core;

namespace SudokuSolver.CLI.OutputHandlers;

/// <summary>
/// Output handler for stdout.
/// </summary>
public class StdoutOutputHandler(ILoggerFactory loggerFactory, bool prettyPrint) : BaseStringOutputHandler(loggerFactory.GetLogger(nameof(StdoutOutputHandler)), prettyPrint)
{

    /// <summary>
    /// Handles the output of a Sudoku puzzle to stdout.
    /// </summary>
    /// <param name="sudoku">The Sudoku puzzle to handle.</param>
    public override void Handle(Sudoku sudoku) => Console.WriteLine(SudokuToString(sudoku));
}