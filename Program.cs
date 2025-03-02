using JackBlog.Models;
using JackBlog.Services;
using Moq;
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
builder.Services.AddSingleton<JackBlog.Models.BlogService>();
builder.Services.AddSingleton<ILogger<CodePuzzleService>>(new ConsoleLogger<CodePuzzleService>());
builder.Services
  .AddSingleton<ILogger<TestCaseProvider<SordidArraysTestCase, SordidArraysInput, double>>>(
      new ConsoleLogger<TestCaseProvider<SordidArraysTestCase, SordidArraysInput, double>>());
builder.Services.AddSingleton<ILogger<CodePuzzleService>>(new ConsoleLogger<CodePuzzleService>());
builder.Services.AddSingleton<ILogger<BlogService>>(new ConsoleLogger<BlogService>());
builder.Services.AddKeyedSingleton<
    ITestCaseProvider<SordidArraysTestCase, SordidArraysInput, double>
>(
    "SordidArrays",
    (serviceProvider, key) =>
        new TestCaseProvider<SordidArraysTestCase, SordidArraysInput, double>(
            key?.ToString()!,
            serviceProvider.GetRequiredService<
                ILogger<TestCaseProvider<SordidArraysTestCase, SordidArraysInput, double>>
            >()
        )
);
builder.Services.AddSingleton<CodePuzzleService>();

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

app.UseRouting();

app.UseAuthorization();

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
