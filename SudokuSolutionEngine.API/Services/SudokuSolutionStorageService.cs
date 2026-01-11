using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using SudokuSolutionEngine.API.Constants;
using SudokuSolutionEngine.API.Models;
using System.Text.Json;

namespace SudokuSolutionEngine.API.Services;

/// <summary>
/// Implementation of ISudokuSolutionStorageService using DynamoDB.
/// </summary>
public class SudokuSolutionStorageService(IAmazonDynamoDB dynamoDb, ILogger<SudokuSolutionStorageService> logger, DynamoDbConfiguration config) : ISudokuSolutionStorageService
{
    public async Task<StoredSudokuSolution?> GetStoredSolutionAsync(string boardHash)
    {
        var request = new GetItemRequest
        {
            TableName = config.TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { DynamoDb.HashKeyName, new AttributeValue { S = boardHash } }
            }
        };

        GetItemResponse response;
        try
        {
            response = await dynamoDb.GetItemAsync(request);
        }
        catch (Exception ex)
        {
            logger.LogError("Error retrieving stored solution for board hash {Hash}: {Exception}", boardHash, ex.Message);
            return null;
        }

        if (response.Item.Count == 0)
        {
            logger.LogDebug("No stored solution found for board hash {Hash}", boardHash);
            return null;
        }

        var stored = CreateStoredSolutionFromItem(boardHash, response.Item);
        logger.LogInformation("Retrieved stored solution for board hash {Hash}", boardHash);
        return stored;
    }

    public async Task StoreSolutionAsync(string boardHash, List<List<byte>>? solvedBoard)
    {
        var item = CreateDynamoDbItem(boardHash, solvedBoard);
        var request = new PutItemRequest
        {
            TableName = config.TableName,
            Item = item
        };

        try
        {
            await dynamoDb.PutItemAsync(request);
            logger.LogInformation("Stored solution for board hash {Hash}", boardHash);
        }
        catch (Exception ex)
        {
            logger.LogError("Error storing solution for board hash {Hash}: {Exception}", boardHash, ex.Message);
        }
    }

    private Dictionary<string, AttributeValue> CreateDynamoDbItem(string boardHash, List<List<byte>>? solvedBoard)
    {
        var storedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var item = new Dictionary<string, AttributeValue>
        {
            { DynamoDb.HashKeyName, new AttributeValue { S = boardHash } },
            { DynamoDb.SolvableAttributeName, new AttributeValue { BOOL = solvedBoard != null } },
            { DynamoDb.SolutionAttributeName, new AttributeValue { S = JsonSerializer.Serialize(solvedBoard) } },
            { DynamoDb.StoredAtAttributeName, new AttributeValue { N = storedAt.ToString() } }
        };

        if (config.TTL.Enabled)
        {
            var ttl = DateTimeOffset.UtcNow.Add(config.TTL.Duration).ToUnixTimeSeconds();
            item[DynamoDb.TTLAttributeName] = new AttributeValue { N = ttl.ToString() };
        }

        return item;
    }

    private static StoredSudokuSolution CreateStoredSolutionFromItem(string boardHash, Dictionary<string, AttributeValue> item)
    {
        var stored = new StoredSudokuSolution
        {
            BoardHash = boardHash,
            Solvable = item.ContainsKey(DynamoDb.SolvableAttributeName)
                ? item[DynamoDb.SolvableAttributeName].BOOL
                : false,
            Solution = item.ContainsKey(DynamoDb.SolutionAttributeName)
                ? item[DynamoDb.SolutionAttributeName].S ?? string.Empty
                : string.Empty,
            StoredAt = item.ContainsKey(DynamoDb.StoredAtAttributeName) &&
                       long.TryParse(item[DynamoDb.StoredAtAttributeName].N, out var storedAt)
                ? storedAt
                : DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        return stored;
    }
}
