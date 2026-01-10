using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SudokuSolutionEngine.CLI;

public interface ILoggerFactory {
    ILogger GetLogger(string categoryName);
}