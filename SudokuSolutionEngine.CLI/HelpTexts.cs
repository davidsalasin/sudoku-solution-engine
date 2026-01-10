using SudokuSolutionEngine.CLI;

public static class HelpTexts
{
    public const string InputTypeHelpText = "The type of the input Sudoku puzzle. Valid values: Text, TextFile (default: Text)";
    public const string InputHelpText = "The input Sudoku puzzle";
    public const string OutputTypeHelpText = "The type of the output. Valid values: Stdout, TextFile (default: Stdout)";
    public const string OutputPathHelpText = "The output file path. Relevant only for TextFile output type";
    public const string PrettyPrintHelpText = "Enable pretty printing of the output. Relevant only for Stdout and TextFile output types (default: false)";
    public const string ConsoleLoggingHelpText = "Enable logging to console (default: false)";
}
