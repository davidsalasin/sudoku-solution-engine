using CommandLine;

namespace SudokuSolver.CLI;

class Options
{
    [Value(0, Required = true, HelpText = "The text to print")]
    public string Input { get; set; } = string.Empty;

    [Option('c', "capital", HelpText = "Capitalize the input text")]
    public bool Capital { get; set; }
}