# API Usage Guide

This guide explains how to use the Sudoku Solution Engine REST API.

## Prerequisites

- .NET SDK 10.0 or later
- For local development: API runs on HTTP (TLS not enabled locally)
- For production: TLS/HTTPS should be handled by your cloud provider (e.g., AWS ALB, Azure Application Gateway)

## Running the API

### Local Development

```bash
# From the solution root directory
dotnet run --project SudokuSolutionEngine.API

# Or with a specific launch profile
dotnet run --project SudokuSolutionEngine.API --launch-profile dev
```

The API will start on:

- **HTTP**: `http://localhost:5000` (default port, configurable)
- **Host**: Binds to all interfaces (`*`) by default

### Configuration

The API uses YAML configuration files located in `SudokuSolutionEngine.API/Configuration/`:

- `appsettings.yaml` - Base configuration
- `appsettings.Development.yaml` - Development overrides

You can configure the host and port via:

1. **Configuration files** (YAML):

   ```yaml
   Host: "*" # or "localhost" for local-only access
   Port: 5000
   ```

2. **Environment variable**:

   ```bash
   export PORT=8080
   ```

3. **Command-line arguments** (passed to the application)

## TLS/HTTPS Configuration

### Local Development

**TLS/HTTPS is NOT enabled in local development.** The API runs on HTTP only for simplicity and because:

- Development certificates require manual setup and trust
- Local development typically doesn't require encryption
- Simplifies local testing and debugging

**Access the API using HTTP:**

```
http://localhost:5000/sudoku
```

### Production Deployment

**TLS/HTTPS termination should be handled by your cloud provider**, not by the application itself. This is the recommended approach for containerized applications:

#### AWS ECS/ALB

- Application Load Balancer (ALB) terminates TLS/HTTPS
- ALB forwards HTTP traffic to your container
- Your container listens on HTTP (typically port 80)
- Configure SSL certificate in ALB listener

#### Other Cloud Providers

- Use reverse proxy/load balancer (nginx, Traefik, etc.) to terminate TLS
- Application container runs HTTP-only
- Reverse proxy handles SSL certificates and HTTPS

**Why this approach?**

- ✅ Centralized certificate management
- ✅ Better performance (TLS termination at edge)
- ✅ Easier certificate rotation
- ✅ Standard practice for containerized applications
- ✅ Application focuses on business logic, not infrastructure

## API Endpoints

### Solve Sudoku Puzzle

**Endpoint:** `POST /sudoku`

**Request Body:**

```json
{
  "board": [
    [0, 0, 3, 0, 2, 0, 6, 0, 0],
    [9, 0, 0, 3, 0, 5, 0, 0, 1],
    [0, 0, 1, 8, 0, 6, 4, 0, 0],
    [0, 0, 8, 1, 0, 2, 9, 0, 0],
    [7, 0, 0, 0, 0, 0, 0, 0, 8],
    [0, 0, 6, 7, 0, 8, 2, 0, 0],
    [0, 0, 2, 6, 0, 9, 5, 0, 0],
    [8, 0, 0, 2, 0, 3, 0, 0, 9],
    [0, 0, 5, 0, 1, 0, 3, 0, 0]
  ]
}
```

**Response (Success - 200 OK):**

```json
{
  "solved": true,
  "board": [
    [4, 8, 3, 9, 2, 1, 6, 5, 7],
    [9, 6, 7, 3, 4, 5, 8, 2, 1],
    [2, 5, 1, 8, 7, 6, 4, 9, 3],
    [5, 4, 8, 1, 3, 2, 9, 7, 6],
    [7, 2, 9, 5, 6, 4, 1, 3, 8],
    [1, 3, 6, 7, 9, 8, 2, 4, 5],
    [3, 7, 2, 6, 8, 9, 5, 1, 4],
    [8, 1, 4, 2, 5, 3, 7, 6, 9],
    [6, 9, 5, 4, 1, 7, 3, 8, 2]
  ],
  "timeMs": 45,
  "error": null
}
```

**Response (Unsolvable - 422 Unprocessable Entity):**

```json
{
  "solved": false,
  "board": null,
  "timeMs": 12,
  "error": null
}
```

**Response (Validation Error - 400 Bad Request):**

```json
{
  "solved": false,
  "board": null,
  "timeMs": 5,
  "error": "Couldn't create Sudoku: (Puzzle length)^0.25 isn't a natural number: 1.189..."
}
```

**Response (Server Error - 500 Internal Server Error):**

```json
{
  "solved": false,
  "board": null,
  "timeMs": 10,
  "error": "An unexpected error occurred"
}
```

## API Documentation (Development Only)

When running in Development mode, interactive API documentation is available:

- **OpenAPI/Swagger JSON**: `http://localhost:5000/openapi/v1.json`
- **Scalar UI**: Available at the configured Scalar endpoint (check console output)

## Board Format

- The board is represented as a 2D array (list of lists) of bytes
- `0` represents an empty cell
- Valid numbers are `1` through `N` where `N` is the side length of the puzzle
- Supported puzzle sizes: 4x4, 9x9, 16x16, 25x25, etc. (any perfect square)

## Error Handling

The API returns appropriate HTTP status codes:

- `200 OK` - Puzzle solved successfully
- `400 Bad Request` - Invalid input (malformed board, invalid puzzle size, etc.)
- `422 Unprocessable Entity` - Valid input but puzzle is unsolvable
- `500 Internal Server Error` - Unexpected server error

All error responses include an `error` field with a descriptive message.

## Performance

- Response includes `timeMs` field showing solve time in milliseconds
- Performance depends on puzzle difficulty and size

## Security Considerations

### Local Development

- HTTP only (no encryption)
- Suitable for local testing only
- Do not expose to public networks

### Production

- Always use HTTPS/TLS (handled by cloud provider)
- Implement authentication/authorization as needed
- Use environment variables for sensitive configuration
- Follow cloud provider security best practices
