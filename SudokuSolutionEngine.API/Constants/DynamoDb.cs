namespace SudokuSolutionEngine.API.Constants;

/// <summary>
/// Constants for DynamoDB table attribute names and keys.
/// </summary>
public static class DynamoDb
{
    /// <summary>
    /// The hash key attribute name for the DynamoDB table.
    /// </summary>
    public const string HashKeyName = "boardHash";

    /// <summary>
    /// The attribute name indicating whether a Sudoku board is solvable.
    /// </summary>
    public const string SolvableAttributeName = "solvable";

    /// <summary>
    /// The attribute name containing the serialized solution board.
    /// </summary>
    public const string SolutionAttributeName = "solution";

    /// <summary>
    /// The attribute name for the timestamp when the solution was stored.
    /// </summary>
    public const string StoredAtAttributeName = "storedAt";

    /// <summary>
    /// The attribute name for the Time To Live (TTL) value.
    /// </summary>
    public const string TTLAttributeName = "ttl";

    /// <summary>
    /// Maximum time to wait for a DynamoDB table to become active after creation.
    /// </summary>
    public static readonly TimeSpan MaxWaitTimeForTableActive = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Interval between polling attempts when waiting for a DynamoDB table to become active.
    /// </summary>
    public static readonly TimeSpan PollIntervalForTableStatus = TimeSpan.FromSeconds(2);

    /// <summary>
    /// Default DynamoDB table name.
    /// </summary>
    public const string DefaultTableName = "SudokuSolutions";

    /// <summary>
    /// Default value for TTL (Time To Live) enabled setting.
    /// </summary>
    public const bool DefaultTTLEnabled = true;

    /// <summary>
    /// Default duration for TTL (Time To Live) expiration.
    /// </summary>
    public static readonly TimeSpan DefaultTTLDuration = TimeSpan.FromDays(30);

    /// <summary>
    /// Default timeout in milliseconds for DynamoDB client operations.
    /// </summary>
    public const int DefaultTimeoutMs = 5000;
}
