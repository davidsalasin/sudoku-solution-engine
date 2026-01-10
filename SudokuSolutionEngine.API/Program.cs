using SudokuSolutionEngine.Core;
using Scalar.AspNetCore;
using SudokuSolutionEngine.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure YAML configuration files
builder.ConfigureYamlConfiguration(args);

// Configure server URLs (host and port)
builder.ConfigureServerUrls();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ISudokuSolverFactory, SudokuSolverFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();
