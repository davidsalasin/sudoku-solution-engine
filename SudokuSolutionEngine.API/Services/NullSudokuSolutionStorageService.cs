using SudokuSolutionEngine.API.Models;

namespace SudokuSolutionEngine.API.Services;

/// <summary>
/// Null implementation of ISudokuSolutionStorageService used when DynamoDB is disabled.
/// </summary>
public class NullSudokuSolutionStorageService : ISudokuSolutionStorageService
{
    public Task<StoredSudokuSolution?> GetStoredSolutionAsync(string boardHash)
    {
        return Task.FromResult<StoredSudokuSolution?>(null);
    }

    public Task StoreSolutionAsync(string boardHash, List<List<byte>>? solvedBoard)
    {
        return Task.CompletedTask;
    }
}
