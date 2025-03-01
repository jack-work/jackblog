# JackBlog Project Guide

## Commands
- **Build**: `dotnet build`
- **Run**: `dotnet run`
- **Run with file watcher**: `DOTNET_USE_POLLING_FILE_WATCHER=1 dotnet run`
- **Publish**: `dotnet publish -c Release`
- **Clean**: `dotnet clean`

## Code Style Guidelines
- **Naming**: PascalCase for classes, methods, properties; camelCase for variables
- **Formatting**: 4-space indentation, opening braces on same line
- **Nullability**: Nullable reference types enabled, use `?` operator for nullable types
- **Imports**: Group System namespaces first, then third-party, then project-specific
- **Error Handling**: Use nullable references and pattern matching for null checks
- **Models**: Keep models simple with properties and minimal logic
- **Views**: Use tag helpers instead of HTML helpers when possible
- **CSS**: Follow BEM methodology for CSS naming
- **Layout**: Maintain separation of concerns (models, views, controllers)
- **Documentation**: XML comments for public APIs