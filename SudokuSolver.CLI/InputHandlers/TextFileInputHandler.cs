using Microsoft.Extensions.Logging;

namespace SudokuSolver.CLI.InputHandlers;

public class TextFileInputHandler(ILoggerFactory loggerFactory) : BaseStringInputHandler(loggerFactory.GetLogger(nameof(TextFileInputHandler)))
{
    public override IList<int> Handle(string input)
    {
        using var reader = new StreamReader(input);
        input = reader.ReadToEnd();
        return StringToIntegerList(input);
    }
}