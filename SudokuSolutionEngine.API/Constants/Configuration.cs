using System.IO;

namespace SudokuSolutionEngine.API.Constants;

public static class Configuration
{
    // Configuration file names
    public const string AppSettingsFileName = "appsettings.yaml";
    public const string AppSettingsEnvironmentFileNameTemplate = "appsettings.{0}.yaml";

    // Configuration folder
    public const string ConfigurationFolder = "Configuration";

    // File paths (OS-safe using Path.Join)
    public static readonly string AppSettingsPath = Path.Join(ConfigurationFolder, AppSettingsFileName);
    public static readonly string AppSettingsEnvironmentPathTemplate = Path.Join(ConfigurationFolder, AppSettingsEnvironmentFileNameTemplate);
}
