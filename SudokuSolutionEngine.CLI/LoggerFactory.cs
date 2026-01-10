using ILogger = Microsoft.Extensions.Logging.ILogger;
using Serilog;
using Serilog.Extensions.Logging;

namespace SudokuSolutionEngine.CLI;

public class LoggerFactory : ILoggerFactory {

    private readonly SerilogLoggerFactory loggerFactory;

    public LoggerFactory(bool enabled) {
        var loggerConfiguration = new LoggerConfiguration();
        if (enabled) {
            loggerConfiguration.WriteTo.Console();
        }
        this.loggerFactory = new SerilogLoggerFactory(loggerConfiguration.CreateLogger());
    }

    public ILogger GetLogger(string categoryName) {
        return this.loggerFactory.CreateLogger(categoryName);
    }
}