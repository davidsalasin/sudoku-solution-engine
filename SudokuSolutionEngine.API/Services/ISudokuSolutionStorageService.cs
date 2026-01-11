using SudokuSolutionEngine.API.Models;

namespace SudokuSolutionEngine.API.Services;

/// <summary>
/// Service for storing Sudoku solutions in DynamoDB.
/// </summary>
public interface ISudokuSolutionStorageService
{
    /// <summary>
    /// Retrieves a stored solution for the given board hash.
    /// </summary>
    /// <param name="boardHash">The hash of the board state.</param>
    /// <returns>The stored solution if found, null otherwise.</returns>
    Task<StoredSudokuSolution?> GetStoredSolutionAsync(string boardHash);

    /// <summary>
    /// Stores a solution for the given board hash.
    /// </summary>
    /// <param name="boardHash">The hash of the board state.</param>
    /// <param name="solvedBoard">The solved board, or null if not solvable.</param>
    Task StoreSolutionAsync(string boardHash, List<List<byte>>? solvedBoard);
}
