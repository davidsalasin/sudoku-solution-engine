namespace SudokuSolver.CLI.InputHandlers;

/// <summary>
/// Interface for input handlers.
/// </summary>
public interface IInputHandler
{
    /// <summary>
    /// Handles the input and returns a list of integers.
    /// </summary>
    /// <param name="input">The input to handle.</param>
    /// <returns>A list of integers.</returns>
    IList<int> Handle(string input);
}