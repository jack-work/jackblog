using System.Text.Json;
using JackBlog.Models;
using System.Diagnostics;

namespace JackBlog.Services;

public class TestCaseProvider : ITestCaseProvider
{
    private readonly JsonSerializerOptions _options = new() {
      PropertyNameCaseInsensitive = true
    };
    private readonly ILogger<TestCaseProvider> _logger;
    private readonly Dictionary<string, string> _descriptionCache = new();

    public TestCaseProvider(ILogger<TestCaseProvider> logger)
    {
        _logger = logger;
    }

    public IEnumerable<TTestCase> GetTestCases<TTestCase, TInput, TResult>(string testName) where TTestCase : ITestCase<TInput, TResult>
    {
        _logger.LogInformation("Loading test cases for {TestName}", testName);
        var stopwatch = Stopwatch.StartNew();

        string testCaseFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "TestCases",
            $"{testName}.json"
        );

        _logger.LogDebug("Test case file path: {FilePath}", testCaseFilePath);

        if (!File.Exists(testCaseFilePath))
        {
            _logger.LogError("Test case file not found: {FilePath}", testCaseFilePath);
            return Enumerable.Empty<TTestCase>();
        }

        string json = File.ReadAllText(testCaseFilePath);
        _logger.LogTrace("Read JSON content of length {Length}", json.Length);
        
        _logger.LogDebug("Deserializing test cases with types: TestCase={TestCaseType}, Input={InputType}, Result={ResultType}",
            typeof(TTestCase).Name, typeof(TInput).Name, typeof(TResult).Name);
        
        try
        {
            var testCases = JsonSerializer.Deserialize<IEnumerable<TTestCase>>(json, _options)
                ?? throw new JsonException($"Failed to deserialize test cases from {testCaseFilePath}");
            
            var testCasesList = testCases.ToList();
            stopwatch.Stop();
            _logger.LogInformation("Loaded {Count} test cases in {ElapsedMs}ms", testCasesList.Count, stopwatch.ElapsedMilliseconds);
            
            return testCasesList;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize test cases from {FilePath}", testCaseFilePath);
            throw;
        }
    }
    
    public string GetPuzzleDescription(string testName)
    {
        if (_descriptionCache.TryGetValue(testName, out string? cachedDescription))
        {
            return cachedDescription;
        }
        
        string descriptionFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "TestCases",
            $"{testName}.description.md"
        );
        _logger.LogInformation(descriptionFilePath);
        
        _logger.LogDebug("Description file path: {FilePath}", descriptionFilePath);
        
        if (!File.Exists(descriptionFilePath))
        {
            _logger.LogWarning("Description file not found: {FilePath}", descriptionFilePath);
            // Return a default description if file is not found
            string defaultDescription = $"Solutions for the {testName} puzzle.";
            _descriptionCache[testName] = defaultDescription;
            return defaultDescription;
        }
        
        try
        {
            string description = File.ReadAllText(descriptionFilePath);
            _logger.LogInformation("Loaded description for {TestName}, length: {Length}", testName, description.Length);
            _descriptionCache[testName] = description;
            return description;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read description from {FilePath}", descriptionFilePath);
            string fallbackDescription = $"Solutions for the {testName} puzzle.";
            _descriptionCache[testName] = fallbackDescription;
            return fallbackDescription;
        }
    }
}

