using Microsoft.AspNetCore.Mvc;
using SudokuSolutionEngine.API.Models;
using SudokuSolutionEngine.API.Constants;
using SudokuSolutionEngine.API.Services;
using SudokuSolutionEngine.Core;
using SudokuSolutionEngine.Core.Exceptions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SudokuSolutionEngine.API.Controllers;

/// <summary>
/// Controller for Sudoku solving operations.
/// </summary>
[ApiController]
[Route(Routes.Sudoku)]
public class SudokuController(
    ISudokuSolverFactory solverFactory,
    ISudokuSolutionStorageService storageService,
    ILogger<SudokuController> logger) : ControllerBase
{
    /// <summary>
    /// Solves a Sudoku puzzle.
    /// </summary>
    /// <param name="request">The Sudoku board to solve.</param>
    /// <returns>The solution result with timing information.</returns>
    [HttpPost]
    public async Task<IActionResult> Solve([FromBody] SudokuSolutionRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = new SudokuSolutionResponse();

        try
        {
            // Compute board hash for storage
            var boardHash = ComputeBoardHash(request.Board);

            // Retrieve stored solution (no-op if DynamoDB is disabled)
            var storedSolution = await storageService.GetStoredSolutionAsync(boardHash);
            if (storedSolution != null && storedSolution.Solvable)
            {
                // Deserialize the board from JSON
                var storedBoard = JsonSerializer.Deserialize<List<List<byte>>>(storedSolution.Solution);
                if (storedBoard != null)
                {
                    stopwatch.Stop();
                    response.Solved = true;
                    response.Board = storedBoard as List<List<byte>>;
                    response.TimeMs = (int)stopwatch.ElapsedMilliseconds;
                    response.RetrievedFromStorage = true;
                    return Ok(response);
                }
            }
            else if (storedSolution != null && !storedSolution.Solvable)
            {
                stopwatch.Stop();
                response.Solved = false;
                response.Board = null;
                response.TimeMs = (int)stopwatch.ElapsedMilliseconds;
                response.RetrievedFromStorage = true;
                return UnprocessableEntity(response);
            }

            // Create Sudoku instance
            var sudoku = new Sudoku(request.Board);

            // Solve the puzzle
            var solver = solverFactory.CreateSolver();
            var solved = solver.Solve(sudoku);

            stopwatch.Stop();

            response.Solved = solved;
            response.TimeMs = stopwatch.ElapsedMilliseconds;
            response.RetrievedFromStorage = false;

            if (solved)
            {
                // Convert the solved board back to a list of lists
                var side = sudoku.Side;
                var board = new List<List<byte>>();
                for (int i = 0; i < side; i++)
                {
                    var row = new List<byte>();
                    for (int j = 0; j < side; j++)
                    {
                        row.Add(sudoku.Board[i, j]);
                    }
                    board.Add(row);
                }
                response.Board = board;
                response.Error = null;

                // Store the solution (no-op if DynamoDB is disabled)
                await storageService.StoreSolutionAsync(boardHash, board as List<List<byte>>);

                return Ok(response);
            }
            else
            {
                response.Board = null;
                response.Error = null;

                // Store unsolvable result (no-op if DynamoDB is disabled)
                await storageService.StoreSolutionAsync(boardHash, null); // null = not solvable

                return UnprocessableEntity(response);
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            response.Solved = false;
            response.Board = null;
            response.Error = ex.Message;
            response.TimeMs = stopwatch.ElapsedMilliseconds;
            response.RetrievedFromStorage = false;

            if (ex is SudokuException)
            {
                logger.LogWarning("Sudoku validation error: {ErrorMessage}", ex.Message);
                return BadRequest(response);
            }
            else
            {
                logger.LogError(ex, "Unexpected error solving Sudoku puzzle");
                return StatusCode(500, response);
            }
        }
    }

    /// <summary>
    /// Computes a SHA256 hash of the board state for storage purposes.
    /// </summary>
    private static string ComputeBoardHash(List<List<byte>> board)
    {
        var json = JsonSerializer.Serialize(board);
        var bytes = Encoding.UTF8.GetBytes(json);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
