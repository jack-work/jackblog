using JackBlog.Models;
using JackBlog.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// Configure resource for telemetry
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(serviceName: "JackBlog", serviceVersion: "1.0.0");

// Add OpenTelemetry logging
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(resourceBuilder);
    options.AddConsoleExporter();
});

// Configure logging level
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddFilter("Microsoft", LogLevel.Debug);
builder.Logging.AddFilter("System", LogLevel.Warning);
builder.Logging.AddFilter("JackBlog", LogLevel.Debug);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register options patterns
builder.Services.Configure<PuzzleSettings>(
    builder.Configuration.GetSection(PuzzleSettings.SectionName));
builder.Services.Configure<SiteSettings>(
    builder.Configuration.GetSection(SiteSettings.SectionName));

// Register markdown service
builder.Services.AddSingleton<MarkdownService>();

builder.Services.AddSingleton<JackBlog.Models.PuzzleAggregator>(services =>
{
    var puzzleServices = services.GetServices<ICodePuzzleService>();
    var logger = services.GetRequiredService<ILogger<PuzzleAggregator>>();
    return new PuzzleAggregator(puzzleServices, logger);
});
builder.Services
  .AddSingleton<ILogger<TestCaseProvider>>(
      new ConsoleLogger<TestCaseProvider>());
builder.Services.AddSingleton<ILogger<PuzzleAggregator>>(new ConsoleLogger<PuzzleAggregator>());
builder.Services.AddSingleton<ITestCaseProvider, TestCaseProvider>();

builder.Services.AddCodePuzzleSolver<SordidArraysSolver, SordidArraysTestCase, SordidArraysInput, double>("SordidArrays");
builder.Services.AddCodePuzzleSolver<TrappingRainWaterSolver, TestCase<int[], int>, int[], int>("TrappingRainWater");
builder.Services.AddCodePuzzleSolver<VassalCensusSolver, TestCase<VassalCensusInput, int>, VassalCensusInput, int>("VassalCensus");
builder.Services.AddCodePuzzleSolver<KokoNannerSolver, TestCase<KokoNannerInput, int>, KokoNannerInput, int>("KokoNanner");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure additional static file serving for the Images folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/images"
});

app.UseRouting();

app.UseAuthorization();

// Configures the default routing pattern for the application:
// - Maps URLs to controller actions based on the pattern {controller}/{action}/{id?}
// - Default controller is "Home" if not specified in URL
// - Default action is "Index" if not specified in URL
// - Optional "id" parameter that can be passed to the action method
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

class ConsoleLogger<T> : ILogger<T>
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Console.WriteLine(state);
    }
}

static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodePuzzleSolver<TSolver, TTestCase, TInput, TResult>(this IServiceCollection services, string puzzleName)
        where TSolver : class, ICodePuzzleSolver<TInput, TResult>
        where TTestCase : ITestCase<TInput, TResult>
        => services
        .AddSingleton<ILogger<TSolver>>(new ConsoleLogger<TSolver>())
        .AddSingleton<ICodePuzzleSolver<TInput, TResult>, TSolver>()
        .AddSingleton<ILogger<CodePuzzleService<TTestCase, TInput, TResult>>>(
            new ConsoleLogger<CodePuzzleService<TTestCase, TInput, TResult>>())
        .AddSingleton<ICodePuzzleService>(collection =>
        {
            var solver = collection.GetRequiredService<ICodePuzzleSolver<TInput, TResult>>();
            var resolver = collection.GetRequiredService<ITestCaseProvider>();
            var logger = collection.GetRequiredService<ILogger<CodePuzzleService<TTestCase, TInput, TResult>>>();
            var puzzleSettings = collection.GetRequiredService<IOptions<PuzzleSettings>>();
            return new CodePuzzleService<TTestCase, TInput, TResult>(puzzleName, solver, resolver, logger, puzzleSettings);
        });
}

