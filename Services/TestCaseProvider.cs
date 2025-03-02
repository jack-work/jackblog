using System.Text.Json;
using JackBlog.Models;
using System.Diagnostics;

namespace JackBlog.Services;

public class TestCaseProvider<TTestCase, TInput, TResult> : ITestCaseProvider<TTestCase, TInput, TResult>
    where TTestCase : ITestCase<TInput, TResult>
{
    private readonly string _testName;
    private readonly JsonSerializerOptions _options = new() {
      PropertyNameCaseInsensitive = true
    };
    private readonly ILogger<TestCaseProvider<TTestCase, TInput, TResult>> _logger;

    public TestCaseProvider(string testName, ILogger<TestCaseProvider<TTestCase, TInput, TResult>> logger)
    {
        _testName = testName;
        _logger = logger;
        _logger.LogInformation("TestCaseProvider initialized for test name: {TestName}", testName);
    }

    public IEnumerable<TTestCase> GetTestCases()
    {
        _logger.LogInformation("Loading test cases for {TestName}", _testName);
        var stopwatch = Stopwatch.StartNew();

        string testCaseFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "testCases",
            $"{_testName}.json"
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
