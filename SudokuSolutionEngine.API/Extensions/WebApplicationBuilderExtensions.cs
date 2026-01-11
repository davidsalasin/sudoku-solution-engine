using SudokuSolutionEngine.API.Constants;
using NetEscapades.Configuration.Yaml;
using ConfigConstants = SudokuSolutionEngine.API.Constants.Configuration;
using Amazon.DynamoDBv2;
using SudokuSolutionEngine.API.Models;
using SudokuSolutionEngine.API.Services;
using SudokuSolutionEngine.API.Exceptions;

namespace SudokuSolutionEngine.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureYamlConfiguration(this WebApplicationBuilder builder, string[]? args = null)
    {
        // Clear default JSON sources and configure YAML file provider
        builder.Configuration.Sources.Clear();
        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddYamlFile(ConfigConstants.AppSettingsPath, optional: false, reloadOnChange: true)
            .AddYamlFile(
                string.Format(ConfigConstants.AppSettingsEnvironmentPathTemplate, builder.Environment.EnvironmentName),
                optional: true,
                reloadOnChange: true)
            .AddEnvironmentVariables();

        if (args != null && args.Length > 0)
        {
            builder.Configuration.AddCommandLine(args);
        }

        return builder;
    }

    public static WebApplicationBuilder ConfigureServerUrls(this WebApplicationBuilder builder)
    {
        var host = builder.Configuration[Server.HostConfigurationKey]
            ?? Server.DefaultHost;

        var port = builder.Configuration[Server.PortConfigurationKey]
            ?? Environment.GetEnvironmentVariable(Server.PortEnvironmentVariable)
            ?? Server.DefaultPort.ToString();

        builder.WebHost.UseUrls($"http://{host}:{port}");
        return builder;
    }

    /// <summary>
    /// Configures DynamoDB storage support.
    /// </summary>
    public static WebApplicationBuilder ConfigureDynamoDb(this WebApplicationBuilder builder)
    {
        // Configure DynamoDB settings
        var dynamoDbConfig = builder.Configuration.GetSection(ConfigConstants.DynamoDbSectionName).Get<DynamoDbConfiguration>() ?? new DynamoDbConfiguration();
        builder.Services.AddSingleton(dynamoDbConfig);

        // Configure DynamoDB client
        if (dynamoDbConfig.Enabled)
        {
            builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                // Validate configuration - ServiceURL and Region are mutually exclusive
                if (!string.IsNullOrWhiteSpace(dynamoDbConfig.ServiceURL) &&
                    !string.IsNullOrWhiteSpace(dynamoDbConfig.Region))
                {
                    throw new InvalidDynamoDbConfigurationException(
                        dynamoDbConfig.ServiceURL,
                        dynamoDbConfig.Region);
                }

                var config = new AmazonDynamoDBConfig();

                if (!string.IsNullOrWhiteSpace(dynamoDbConfig.ServiceURL))
                    config.ServiceURL = dynamoDbConfig.ServiceURL;
                else if (!string.IsNullOrWhiteSpace(dynamoDbConfig.Region))
                    config.RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(dynamoDbConfig.Region);

                return new AmazonDynamoDBClient(config);
            });

            // Add storage services
            builder.Services.AddScoped<ISudokuSolutionStorageService, SudokuSolutionStorageService>();
            builder.Services.AddHostedService<DynamoDbInitializer>();
        }
        else
        {
            // Register a no-op implementation when DynamoDB is disabled
            builder.Services.AddScoped<ISudokuSolutionStorageService, NullSudokuSolutionStorageService>();
        }

        return builder;
    }
}
