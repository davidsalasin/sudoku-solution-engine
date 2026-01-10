using Microsoft.Extensions.Logging;

namespace SudokuSolutionEngine.CLI.InputHandlers;

public class TextFileInputHandler(ILoggerFactory loggerFactory) : BaseStringInputHandler(loggerFactory.GetLogger(nameof(TextFileInputHandler)))
{
    public override IList<byte> Handle(string input)
    {
        using var reader = new StreamReader(input);
        input = reader.ReadToEnd();
        return StringToByteList(input);
    }
}