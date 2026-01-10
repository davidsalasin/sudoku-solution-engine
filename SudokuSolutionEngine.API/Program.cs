using SudokuSolutionEngine.Core;
using Scalar.AspNetCore;
using SudokuSolutionEngine.API.Extensions;
using SudokuSolutionEngine.API.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureYamlConfiguration(args);
builder.ConfigureServerUrls();

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
