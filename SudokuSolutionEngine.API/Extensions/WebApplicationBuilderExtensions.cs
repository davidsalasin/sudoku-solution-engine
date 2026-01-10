using SudokuSolutionEngine.API.Constants;
using NetEscapades.Configuration.Yaml;
using ConfigConstants = SudokuSolutionEngine.API.Constants.Configuration;

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
}
