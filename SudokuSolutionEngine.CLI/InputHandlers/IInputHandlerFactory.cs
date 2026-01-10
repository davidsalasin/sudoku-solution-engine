namespace SudokuSolutionEngine.CLI.InputHandlers;

/// <summary>
/// Interface for input handler factory.
/// </summary>
public interface IInputHandlerFactory
{
    /// <summary>
    /// Creates a new input handler.
    /// </summary>
    /// <param name="inputType">The type of input to create a handler for.</param>
    /// <returns>A new input handler.</returns>
    IInputHandler CreateHandler(InputType inputType);
}