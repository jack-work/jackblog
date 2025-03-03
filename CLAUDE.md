# JackBlog Project Guide

## Commands
- **Build**: `dotnet build`
- **Run**: `dotnet run`
- **Run with file watcher**: `DOTNET_USE_POLLING_FILE_WATCHER=1 dotnet run`
- **Publish**: `dotnet publish -c Release`
- **Clean**: `dotnet clean`
- **Test a specific puzzle**: Use the UI or implement `CodePuzzleService.Solve()` with specific puzzle name

## Code Style Guidelines
- **Naming**: PascalCase for classes, methods, properties; camelCase for variables
- **Formatting**: 4-space indentation, opening braces on same line
- **Nullability**: Nullable reference types enabled, use `?` operator for nullable types
- **Imports**: Group System namespaces first, then third-party, then project-specific
- **Error Handling**: Use nullable references and pattern matching for null checks
- **Services**: Follow interface-based design with `ICodePuzzleService`, `ICodePuzzleSolver`
- **Models**: Keep models simple with properties and minimal logic
- **Test Cases**: Define in JSON files in TestCases directory
- **Puzzles**: Each puzzle should implement solver in its own directory under Services/Puzzles
- **Documentation**: XML comments for public APIs and Markdown for puzzle descriptions