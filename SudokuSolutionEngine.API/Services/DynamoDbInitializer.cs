using System.Linq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using SudokuSolutionEngine.API.Constants;
using SudokuSolutionEngine.API.Exceptions;
using SudokuSolutionEngine.API.Models;

namespace SudokuSolutionEngine.API.Services;

/// <summary>
/// Service that initializes the DynamoDB table on application startup.
/// </summary>
public class DynamoDbInitializer(IAmazonDynamoDB dynamoDb, ILogger<DynamoDbInitializer> logger, DynamoDbConfiguration config) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!config.Enabled)
        {
            logger.LogInformation("DynamoDB storage is disabled, skipping table initialization");
            return;
        }

        var tableDescription = await GetTableDescriptionAsync(cancellationToken);

        if (tableDescription == null)
        {
            logger.LogInformation("DynamoDB table {TableName} does not exist, creating it", config.TableName);
            await CreateTableAsync(cancellationToken);
            logger.LogInformation("DynamoDB table {TableName} created and is now active", config.TableName);
        }
        else
        {
            logger.LogInformation("DynamoDB table {TableName} already exists, validating schema", config.TableName);
            await ValidateTableSchemaAsync(tableDescription, cancellationToken);
            logger.LogInformation("DynamoDB table {TableName} exists and schema is valid", config.TableName);
        }
    }

    private async Task<TableDescription?> GetTableDescriptionAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await dynamoDb.DescribeTableAsync(config.TableName, cancellationToken);
            return response.Table;
        }
        catch (ResourceNotFoundException)
        {
            return null;
        }
    }

    private async Task ValidateTableSchemaAsync(TableDescription table, CancellationToken cancellationToken)
    {
        var validationErrors = new List<string>();

        // Validate hash key exists and has correct name
        var hashKey = table.KeySchema.FirstOrDefault(k => k.KeyType == KeyType.HASH);
        if (hashKey == null)
        {
            validationErrors.Add($"Table has no hash key. Expected hash key: {DynamoDb.HashKeyName}");
        }
        else
        {
            if (hashKey.AttributeName != DynamoDb.HashKeyName)
            {
                validationErrors.Add(
                    $"Hash key has incorrect name. Expected: {DynamoDb.HashKeyName}, Found: {hashKey.AttributeName}");
            }

            // Validate hash key attribute type
            var hashKeyAttribute = table.AttributeDefinitions
                .FirstOrDefault(a => a.AttributeName == DynamoDb.HashKeyName);
            if (hashKeyAttribute == null)
            {
                validationErrors.Add(
                    $"Hash key attribute {DynamoDb.HashKeyName} is not defined in AttributeDefinitions");
            }
            else if (hashKeyAttribute.AttributeType != ScalarAttributeType.S)
            {
                validationErrors.Add(
                    $"Hash key has incorrect type. Expected: {ScalarAttributeType.S}, Found: {hashKeyAttribute.AttributeType}");
            }
        }

        // Validate TTL configuration
        var ttlResponse = await dynamoDb.DescribeTimeToLiveAsync(new DescribeTimeToLiveRequest
        {
            TableName = config.TableName
        }, cancellationToken);

        var ttlStatus = ttlResponse.TimeToLiveDescription;
        if (ttlStatus?.TimeToLiveStatus != TimeToLiveStatus.ENABLED)
        {
            validationErrors.Add(
                $"TTL is not enabled on the table. Expected TTL to be enabled with attribute: {DynamoDb.TTLAttributeName}");
        }
        else if (ttlStatus.AttributeName != DynamoDb.TTLAttributeName)
        {
            validationErrors.Add(
                $"TTL is enabled but with incorrect attribute. Expected: {DynamoDb.TTLAttributeName}, Found: {ttlStatus.AttributeName}");
        }

        // Warn (but don't fail) if billing mode is different
        if (table.BillingModeSummary?.BillingMode != BillingMode.PAY_PER_REQUEST)
        {
            logger.LogWarning(
                "Table {TableName} exists but has billing mode {BillingMode}, expected {ExpectedMode}. " +
                "This may cause unexpected costs.",
                config.TableName, table.BillingModeSummary?.BillingMode, BillingMode.PAY_PER_REQUEST);
        }

        // Throw aggregated exception if any validation errors were found
        if (validationErrors.Count > 0)
        {
            throw new DynamoDbSchemaValidationException(config.TableName, validationErrors);
        }
    }

    private async Task CreateTableAsync(CancellationToken cancellationToken)
    {
        var request = new CreateTableRequest
        {
            TableName = config.TableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    AttributeName = DynamoDb.HashKeyName,
                    AttributeType = ScalarAttributeType.S
                }
            },
            KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement
                {
                    AttributeName = DynamoDb.HashKeyName,
                    KeyType = KeyType.HASH
                }
            },
            BillingMode = BillingMode.PAY_PER_REQUEST
        };

        await dynamoDb.CreateTableAsync(request, cancellationToken);
        await WaitForTableToBeActiveAsync(cancellationToken);

        // Configure TTL after table is active (TTL cannot be set during table creation)
        await ConfigureTimeToLiveAsync(cancellationToken);
    }

    private async Task WaitForTableToBeActiveAsync(CancellationToken cancellationToken)
    {
        var startTime = DateTime.UtcNow;

        while (DateTime.UtcNow - startTime < DynamoDb.MaxWaitTimeForTableActive)
        {
            try
            {
                var response = await dynamoDb.DescribeTableAsync(config.TableName, cancellationToken);
                if (response.Table.TableStatus == TableStatus.ACTIVE)
                {
                    return;
                }

                await Task.Delay(DynamoDb.PollIntervalForTableStatus, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning("Error checking table status: {Exception}, retrying...", ex.Message);
                await Task.Delay(DynamoDb.PollIntervalForTableStatus, cancellationToken);
            }
        }

        throw new TimeoutException(
            $"Timeout waiting for DynamoDB table {config.TableName} to become active after {DynamoDb.MaxWaitTimeForTableActive.TotalMinutes} minutes");
    }

    private async Task ConfigureTimeToLiveAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Check current TTL status
            var ttlResponse = await dynamoDb.DescribeTimeToLiveAsync(new DescribeTimeToLiveRequest
            {
                TableName = config.TableName
            }, cancellationToken);

            var currentTtlStatus = ttlResponse.TimeToLiveDescription;

            if (currentTtlStatus?.TimeToLiveStatus != TimeToLiveStatus.ENABLED ||
                currentTtlStatus?.AttributeName != DynamoDb.TTLAttributeName)
            {
                logger.LogInformation(
                    "Configuring TTL for table {TableName} with attribute {AttributeName}",
                    config.TableName, DynamoDb.TTLAttributeName);

                await dynamoDb.UpdateTimeToLiveAsync(new UpdateTimeToLiveRequest
                {
                    TableName = config.TableName,
                    TimeToLiveSpecification = new TimeToLiveSpecification
                    {
                        Enabled = true,
                        AttributeName = DynamoDb.TTLAttributeName
                    }
                }, cancellationToken);

                logger.LogInformation(
                    "TTL configured successfully for table {TableName} with attribute {AttributeName}. " +
                    "Items will expire when TTL attribute is set with a past timestamp.",
                    config.TableName, DynamoDb.TTLAttributeName);
            }
            else
            {
                logger.LogInformation(
                    "TTL is already enabled for table {TableName} with attribute {AttributeName}",
                    config.TableName, DynamoDb.TTLAttributeName);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error configuring TTL for table {TableName}", config.TableName);
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
