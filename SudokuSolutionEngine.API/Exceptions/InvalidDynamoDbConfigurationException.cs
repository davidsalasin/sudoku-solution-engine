namespace SudokuSolutionEngine.API.Exceptions;

/// <summary>
/// Exception thrown when DynamoDB configuration has conflicting settings.
/// ServiceURL and Region are mutually exclusive - ServiceURL is used for custom endpoints
/// (LocalStack, DynamoDB Local, etc.), while Region is used for AWS DynamoDB.
/// </summary>
/// <remarks>
/// Initializes a new instance of the InvalidDynamoDbConfigurationException class.
/// </remarks>
/// <param name="serviceUrl">The ServiceURL that was specified in the configuration.</param>
/// <param name="region">The Region that was specified in the configuration.</param>
public class InvalidDynamoDbConfigurationException(string? serviceUrl, string? region)
    : SudokuSolutionEngineApiException(
        $"Invalid DynamoDB configuration: Cannot specify both ServiceURL and Region. " +
        $"ServiceURL is used for custom endpoints (LocalStack, DynamoDB Local, etc.), " +
        $"while Region is used for AWS DynamoDB. " +
        $"ServiceURL: '{serviceUrl ?? "null"}', Region: '{region ?? "null"}'")
{
    /// <summary>
    /// Gets the ServiceURL that was specified in the configuration.
    /// </summary>
    public string? ServiceUrl { get; } = serviceUrl;

    /// <summary>
    /// Gets the Region that was specified in the configuration.
    /// </summary>
    public string? Region { get; } = region;
}
