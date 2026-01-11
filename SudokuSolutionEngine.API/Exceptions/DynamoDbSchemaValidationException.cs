using System.Linq;

namespace SudokuSolutionEngine.API.Exceptions;

/// <summary>
/// Exception thrown when a DynamoDB table exists but has a schema that doesn't match the expected schema.
/// </summary>
/// <remarks>
/// Initializes a new instance of the DynamoDbSchemaValidationException class.
/// </remarks>
/// <param name="tableName">The name of the table that failed validation.</param>
/// <param name="validationErrors">The list of validation errors found.</param>
public class DynamoDbSchemaValidationException(string tableName, IReadOnlyList<string> validationErrors)
    : SudokuSolutionEngineApiException(
        $"DynamoDB table '{tableName}' exists but has schema validation errors:{Environment.NewLine}" +
        string.Join(Environment.NewLine, validationErrors.Select((error, index) => $"  {index + 1}. {error}")))
{
    /// <summary>
    /// Gets the name of the table that failed validation.
    /// </summary>
    public string TableName { get; } = tableName;

    /// <summary>
    /// Gets the list of validation errors found.
    /// </summary>
    public IReadOnlyList<string> ValidationErrors { get; } = validationErrors;
}
