namespace SudokuSolutionEngine.CLI.InputHandlers;

/// <summary>
/// Interface for input handlers.
/// </summary>
public interface IInputHandler
{
    /// <summary>
    /// Handles the input and returns a list of bytes.
    /// </summary>
    /// <param name="input">The input to handle.</param>
    /// <returns>A list of bytes.</returns>
    IList<byte> Handle(string input);
}