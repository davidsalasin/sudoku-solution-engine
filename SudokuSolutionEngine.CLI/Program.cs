
using CommandLine;
using Microsoft.Extensions.Logging;
using SudokuSolutionEngine.CLI;
using SudokuSolutionEngine.CLI.InputHandlers;
using SudokuSolutionEngine.CLI.OutputHandlers;
using SudokuSolutionEngine.Core;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(options =>
    {
        var loggerFactory = new SudokuSolutionEngine.CLI.LoggerFactory(options.ConsoleLogging);
        var logger = loggerFactory.GetLogger(nameof(Program));

        var inputHandlerFactory = new InputHandlerFactory(loggerFactory);
        var sudokuSolverFactory = new SudokuSolverFactory();
        var outputHandlerFactory = new OutputHandlerFactory(loggerFactory);

        var facade = new Facade(loggerFactory, inputHandlerFactory, sudokuSolverFactory, outputHandlerFactory);

        try
        {
            facade.Run(options);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Error running Sudoku Solver CLI: {e.Message}");
            Environment.Exit(1);
        }
    });