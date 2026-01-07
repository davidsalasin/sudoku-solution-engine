using Microsoft.Extensions.Logging;

namespace SudokuSolver.CLI.InputHandlers;
public class TextInputHandler(ILoggerFactory loggerFactory) : BaseStringInputHandler(loggerFactory.GetLogger(nameof(TextInputHandler)))
{
    public override IList<int> Handle(string input) => StringToIntegerList(input);
}