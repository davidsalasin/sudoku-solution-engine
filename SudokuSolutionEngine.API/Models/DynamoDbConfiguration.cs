using SudokuSolutionEngine.API.Constants;

namespace SudokuSolutionEngine.API.Models;

/// <summary>
/// Configuration for DynamoDB storage functionality.
/// </summary>
public class DynamoDbConfiguration
{
    /// <summary>
    /// Whether DynamoDB storage is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Optional service URL for DynamoDB-compatible endpoints.
    /// If provided, uses this endpoint (e.g., LocalStack, DynamoDB Local, or other compatible services).
    /// If null or empty, uses AWS DynamoDB with the specified region (or auto-detected region).
    /// Examples:
    /// - LocalStack: "http://localhost:4566"
    /// - DynamoDB Local: "http://localhost:8000"
    /// - AWS: null or empty (uses region-based endpoint)
    /// </summary>
    public string? ServiceURL { get; set; }

    /// <summary>
    /// DynamoDB table name.
    /// </summary>
    public string TableName { get; set; } = DynamoDb.DefaultTableName;

    /// <summary>
    /// AWS region (optional).
    /// If not provided, AWS SDK will auto-detect from:
    /// - Environment variables (AWS_REGION, AWS_DEFAULT_REGION)
    /// - ECS/EC2 instance metadata
    /// - AWS config files
    /// - Defaults to us-east-1 if all else fails
    /// Only set this if you need to override auto-detection.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Timeout in milliseconds for DynamoDB client operations.
    /// </summary>
    public int TimeoutMs { get; set; } = DynamoDb.DefaultTimeoutMs;

    /// <summary>
    /// TTL (Time To Live) configuration.
    /// </summary>
    public TTLConfiguration TTL { get; set; } = new();
}

/// <summary>
/// TTL (Time To Live) configuration for storage expiration.
/// </summary>
public class TTLConfiguration
{
    /// <summary>
    /// Whether TTL is enabled.
    /// </summary>
    public bool Enabled { get; set; } = DynamoDb.DefaultTTLEnabled;

    /// <summary>
    /// Duration before storage entries expire.
    /// </summary>
    public TimeSpan Duration { get; set; } = DynamoDb.DefaultTTLDuration;
}
