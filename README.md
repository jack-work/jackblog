# JackBlog

A blog and code puzzle platform built with ASP.NET Core 8.0. This project showcases blog posts and interactive coding puzzles with test cases and solutions.

## Features

- Clean, sepia-toned UI with responsive design
- Code puzzle platform with test cases and solutions
- Markdown rendering for puzzle descriptions
- Interactive puzzle details with solutions
- Mobile-friendly layout

## Requirements

- .NET SDK 8.0 or later

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the application:

```bash
dotnet run
```

By default, the application will be available at `https://localhost:5001` and `http://localhost:5000`.

## Project Structure

- **Controllers/**: MVC controllers for handling HTTP requests
- **Models/**: Data models for puzzles, site settings, and test cases
  - `PuzzlePost.cs`: Represents a puzzle blog post
  - `PuzzleSettings.cs`: Configuration for puzzles
  - `PuzzleSolution.cs`: Solution implementations for puzzles
  - `ITestCase.cs`, `IResolvedTestCase.cs`: Interfaces for test cases
- **Views/**: Razor views for rendering UI
  - `Home/`: Contains Index and Details views for puzzles
  - `Shared/`: Contains layout templates
- **Services/**: Core business logic
  - `MarkdownService.cs`: Handles markdown rendering
  - `PuzzleAggregator.cs`: Aggregates puzzle data
  - `Puzzles/`: Contains puzzle-specific implementations
    - `CodePuzzleService.cs`: Base service for puzzles
    - `SordidArrays/`: Implementation for the Sordid Arrays puzzle
    - `TrappingRainwater/`: Implementation for the Trapping Rainwater puzzle
- **TestCases/**: JSON test cases and markdown descriptions for puzzles
- **wwwroot/**: Static files (CSS, JS, images)

## Puzzle System Architecture

The application follows a service-oriented architecture for puzzles:

1. Each puzzle implements `ICodePuzzleService` and `ICodePuzzleSolver`
2. Test cases are defined in JSON files in the TestCases directory
3. `PuzzleAggregator` collects all puzzles and makes them available to controllers
4. The `HomeController` presents puzzles and their solutions
5. Puzzle descriptions are written in Markdown and rendered using the `MarkdownService`

## Development

To run the application with file watching enabled:

```bash
DOTNET_USE_POLLING_FILE_WATCHER=1 dotnet run
```

## Build and Deployment

```bash
# Build the project
dotnet build

# Publish for deployment
dotnet publish -c Release
```

## License

This project is open source and available under the [MIT License](LICENSE).