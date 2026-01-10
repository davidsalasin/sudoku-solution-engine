namespace SudokuSolutionEngine.CLI.InputHandlers;

/// <summary>
/// Factory for creating input handlers.
/// </summary>
public class InputHandlerFactory(ILoggerFactory loggerFactory) : IInputHandlerFactory
{
    /// <summary>
    /// Creates a new input handler.
    /// </summary>
    /// <param name="inputType">The type of input to create a handler for.</param>
    /// <returns>A new input handler.</returns>
    public IInputHandler CreateHandler(InputType inputType)
    {
        return inputType switch
        {
            InputType.Text => new TextInputHandler(loggerFactory),
            InputType.TextFile => new TextFileInputHandler(loggerFactory),
            _ => throw new ArgumentException("Invalid input type"),
        };
    }
}