using CommandLine;

namespace SudokuSolutionEngine.CLI;

public class Options
{
    [Option('i', "input-type", HelpText = HelpTexts.InputTypeHelpText)]
    public InputType InputType { get; set; } = InputType.Text;

    [Value(0, Required = true, HelpText = HelpTexts.InputHelpText)]
    public string Input { get; set; } = string.Empty;

    [Option('o', "output-type", HelpText = HelpTexts.OutputTypeHelpText)]
    public OutputType OutputType { get; set; } = OutputType.Stdout;

    [Value(1, Required = false, HelpText = HelpTexts.OutputPathHelpText)]
    public string OutputPath { get; set; } = string.Empty;

    [Option('p', "pretty", HelpText = HelpTexts.PrettyPrintHelpText)]
    public bool PrettyPrint { get; set; } = false;

    [Option('l', "logging", HelpText = HelpTexts.ConsoleLoggingHelpText)]
    public bool ConsoleLogging { get; set; }
}