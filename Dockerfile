# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files (in dependency order)
COPY SudokuSolutionEngine.Core/SudokuSolutionEngine.Core.csproj SudokuSolutionEngine.Core/
COPY SudokuSolutionEngine.API/SudokuSolutionEngine.API.csproj SudokuSolutionEngine.API/

# Restore dependencies for Core first (API depends on it)
WORKDIR /src/SudokuSolutionEngine.Core
RUN dotnet restore

# Restore dependencies for API
WORKDIR /src/SudokuSolutionEngine.API
RUN dotnet restore

# Copy all source files
COPY SudokuSolutionEngine.Core/ SudokuSolutionEngine.Core/
COPY SudokuSolutionEngine.API/ SudokuSolutionEngine.API/

# Build and publish
WORKDIR /src/SudokuSolutionEngine.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application
COPY --from=build /app/publish .

# Copy configuration files
COPY SudokuSolutionEngine.API/Configuration/ ./Configuration/

# Install curl for health checks (must be done as root)
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/*

# Create logs directory with proper permissions
RUN mkdir -p logs && chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development

# Entry point
ENTRYPOINT ["dotnet", "SudokuSolutionEngine.API.dll"]
