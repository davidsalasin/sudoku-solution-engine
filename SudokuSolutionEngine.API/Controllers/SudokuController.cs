using Microsoft.AspNetCore.Mvc;
using SudokuSolutionEngine.API.Models;
using SudokuSolutionEngine.Core;
using SudokuSolutionEngine.Core.Exceptions;

namespace SudokuSolutionEngine.API.Controllers;

/// <summary>
/// Controller for Sudoku solving operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class SudokuController : ControllerBase
{
    private readonly ISudokuSolverFactory _solverFactory;
    private readonly ILogger<SudokuController> _logger;

    public SudokuController(ISudokuSolverFactory solverFactory, ILogger<SudokuController> logger)
    {
        _solverFactory = solverFactory;
        _logger = logger;
    }

    /// <summary>
    /// Solves a Sudoku puzzle.
    /// </summary>
    /// <param name="request">The Sudoku board to solve.</param>
    /// <returns>The solution result with timing information.</returns>
    [HttpPost]
    public IActionResult Solve([FromBody] SudokuRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = new SudokuResponse();

        try
        {
            // Create Sudoku instance
            var boardList = request.Board.Select(row => (IList<byte>)row).ToList();
            var sudoku = new Sudoku(boardList);

            // Solve the puzzle
            var solver = _solverFactory.CreateSolver();
            var solved = solver.Solve(sudoku);

            stopwatch.Stop();

            response.Solved = solved;
            response.TimeMs = (int)stopwatch.ElapsedMilliseconds;

            if (solved)
            {
                // Convert the solved board back to a list of lists
                var side = sudoku.Side;
                response.Board = new List<List<byte>>();
                for (int i = 0; i < side; i++)
                {
                    var row = new List<byte>();
                    for (int j = 0; j < side; j++)
                    {
                        row.Add(sudoku.Board[i, j]);
                    }
                    response.Board.Add(row);
                }
                response.Error = null;
                return Ok(response);
            }
            else
            {
                response.Board = null;
                response.Error = null;
                return UnprocessableEntity(response);
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            response.Solved = false;
            response.Board = null;
            response.Error = ex.Message;
            response.TimeMs = (int)stopwatch.ElapsedMilliseconds;

            if (ex is SudokuException)
            {
                _logger.LogWarning(ex, "Sudoku validation error");
                return BadRequest(response);
            }
            else
            {
                _logger.LogError(ex, "Unexpected error solving Sudoku puzzle");
                return StatusCode(500, response);
            }
        }
    }
}
