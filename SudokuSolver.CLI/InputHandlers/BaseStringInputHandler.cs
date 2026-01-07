using Microsoft.Extensions.Logging;

namespace SudokuSolver.CLI.InputHandlers;

/// <summary>
/// Base class for input handlers that handle input from a string.
/// </summary>
public abstract class BaseStringInputHandler(ILogger logger) : IInputHandler
{
    const string NonNumericCharacterWarning = "Ignoring non-numeric character found in input: {Character}";
    const char MinNumericCharacter = '0';

    /// <summary>
    /// Handles the input and returns a list of integers.
    /// </summary>
    /// <param name="input">The input to handle.</param>
    /// <returns>A list of integers.</returns>
    public abstract IList<int> Handle(string input);

    /// <summary>
    /// Converts a string to a list of integers.
    /// </summary>
    /// <param name="input">The input to convert.</param>
    /// <returns>A list of integers.</returns>
    protected IList<int> StringToIntegerList(string input)
    {
        var inputList = new List<int>();
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] < MinNumericCharacter)
            {
                logger.LogWarning(NonNumericCharacterWarning, input[i]);
                continue;
            }
            inputList.Add(input[i] - MinNumericCharacter);
        }
        return inputList;
    }
}