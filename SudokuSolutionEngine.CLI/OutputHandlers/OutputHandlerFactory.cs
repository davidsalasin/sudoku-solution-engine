namespace SudokuSolutionEngine.CLI.OutputHandlers;

/// <summary>
/// Factory for creating output handlers.
/// </summary>
public class OutputHandlerFactory(ILoggerFactory loggerFactory) : IOutputHandlerFactory
{
    /// <summary>
    /// Creates a new output handler.
    /// </summary>
    /// <param name="outputType">The type of output to create a handler for.</param>
    /// <returns>A new output handler.</returns>
    public IOutputHandler CreateHandler(OutputType outputType, bool prettyPrint, string outputPath) => outputType switch
    {
        OutputType.Stdout => new StdoutOutputHandler(loggerFactory, prettyPrint),
        OutputType.TextFile => new TextFileOutputHandler(loggerFactory, prettyPrint, outputPath),
        _ => throw new ArgumentException("Invalid output type"),
    };
}