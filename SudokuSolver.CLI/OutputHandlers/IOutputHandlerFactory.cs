namespace SudokuSolver.CLI.OutputHandlers;

/// <summary>
/// Interface for output handler factory.
/// </summary>
public interface IOutputHandlerFactory
{
    /// <summary>
    /// Creates a new output handler.
    /// </summary>
    /// <param name="outputType">The type of output to create a handler for.</param>
    /// <param name="prettyPrint">Whether to pretty print the output.</param>
    /// <param name="outputPath">The path to the output file.</param>
    /// <returns>A new output handler.</returns>
    IOutputHandler CreateHandler(OutputType outputType, bool prettyPrint, string outputPath);
}