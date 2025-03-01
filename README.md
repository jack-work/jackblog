# JackBlog

A simple, old-fashioned blog built with ASP.NET Core 8.0. This project demonstrates a basic blog implementation with static entries.

## Features

- Clean, sepia-toned UI with responsive design
- Static blog entries maintained in code
- Full post view with details page
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

- **Controllers/**: Contains MVC controllers
- **Models/**: Contains data models and the blog service
- **Views/**: Contains Razor views
- **wwwroot/**: Contains static files (CSS, JS, etc.)

## Development

To run the application with file watching enabled:

```bash
DOTNET_USE_POLLING_FILE_WATCHER=1 dotnet run
```

## License

This project is open source and available under the [MIT License](LICENSE).