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
            throw new FileNotFoundException($"Test case file not found: {testCaseFilePath}");
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
}

