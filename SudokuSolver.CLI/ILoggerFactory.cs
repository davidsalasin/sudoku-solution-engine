using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SudokuSolver.CLI;

public interface ILoggerFactory {
    ILogger GetLogger(string categoryName);
}