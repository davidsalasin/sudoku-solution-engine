using Microsoft.Extensions.Logging;
using SudokuSolutionEngine.CLI.InputHandlers;
using SudokuSolutionEngine.CLI.OutputHandlers;
using SudokuSolutionEngine.Core;

namespace SudokuSolutionEngine.CLI;

public class Facade(ILoggerFactory loggerFactory, IInputHandlerFactory inputHandlerFactory, ISudokuSolverFactory sudokuSolverFactory, IOutputHandlerFactory outputHandlerFactory)
{
    private readonly ILogger logger = loggerFactory.GetLogger(nameof(Facade));

    public void Run(Options options)
    {
        if (options.OutputType == OutputType.TextFile)
        {
            if (string.IsNullOrEmpty(options.OutputPath))
            {
                throw new ArgumentException("Output file path is required for TextFile output type");
            }
        }

        var inputHandler = inputHandlerFactory.CreateHandler(options.InputType);
        var sudokuSolver = sudokuSolverFactory.CreateSolver();
        var outputHandler = outputHandlerFactory.CreateHandler(options.OutputType, options.PrettyPrint, options.OutputPath);

        var sudokuInput = inputHandler.Handle(options.Input);
        var sudoku = new Sudoku(sudokuInput);
        sudokuSolver.Solve(sudoku);
        outputHandler.Handle(sudoku);
    }
}