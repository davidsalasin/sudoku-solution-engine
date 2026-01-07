
using CommandLine;
using Microsoft.Extensions.Logging;
using SudokuSolver.CLI;
using SudokuSolver.CLI.InputHandlers;
using SudokuSolver.CLI.OutputHandlers;
using SudokuSolver.Core;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(options => {
        var loggerFactory = new SudokuSolver.CLI.LoggerFactory(options.ConsoleLogging);
        var logger = loggerFactory.GetLogger(nameof(Program));

        var inputHandlerFactory = new InputHandlerFactory(loggerFactory);
        var sudokuSolverFactory = new SudokuSolverFactory();
        var outputHandlerFactory = new OutputHandlerFactory(loggerFactory);

        var facade = new Facade(loggerFactory, inputHandlerFactory, sudokuSolverFactory, outputHandlerFactory);

        try {
            facade.Run(options);
        } catch (Exception e) {
            logger.LogError(e, "Error running Sudoku Solver: {Message}", e.Message);
            Environment.Exit(1);
        }
    });