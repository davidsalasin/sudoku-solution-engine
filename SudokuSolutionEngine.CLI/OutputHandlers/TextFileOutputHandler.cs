using SudokuSolutionEngine.Core;

namespace SudokuSolutionEngine.CLI.OutputHandlers;

/// <summary>
/// Output handler for text file.
/// </summary>
public class TextFileOutputHandler(ILoggerFactory loggerFactory, bool prettyPrint, string outputPath) : BaseStringOutputHandler(loggerFactory.GetLogger(nameof(TextFileOutputHandler)), prettyPrint)
{
    /// <summary>
    /// Handles the output of a Sudoku puzzle to text file.
    /// </summary>
    /// <param name="sudoku">The Sudoku puzzle to handle.</param>
    public override void Handle(Sudoku sudoku) => File.WriteAllText(outputPath, SudokuToString(sudoku));
}