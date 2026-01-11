using SudokuSolutionEngine.Core;
using Scalar.AspNetCore;
using SudokuSolutionEngine.API.Extensions;
using SudokuSolutionEngine.API.Constants;
using Serilog;

// Bootstrap logger for early startup logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureYamlConfiguration(args);
    builder.ConfigureSerilog();
    builder.ConfigureServerUrls();
    builder.ConfigureDynamoDb();

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddSingleton<ISudokuSolverFactory, SudokuSolverFactory>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithBundleUrl(SudokuSolutionEngine.API.Constants.Scalar.BundleUrl);
        });
    }

    app.MapControllers();

    app.Run();
}
catch (OperationCanceledException)
{
    // Application was canceled (e.g., Ctrl+C) - this is expected, not an error
    // TaskCanceledException inherits from OperationCanceledException, so this catches both
    Log.Information("Application shutdown requested");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
