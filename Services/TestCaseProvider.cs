using System.Text.Json;
using JackBlog.Models;

namespace JackBlog.Services;

internal class TestCaseProvider<TTestCase, TInput, TResult>(string testName)
    : ITestCaseProvider<TTestCase, TInput, TResult>
    where TTestCase : ITestCase<TInput, TResult>
{
    private readonly JsonSerializerOptions _options = new() {
      PropertyNameCaseInsensitive = true
    };

    public IEnumerable<TTestCase> GetTestCases()
    {
        string testCaseFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "testCases",
            $"{testName}.json"
        );

        Console.WriteLine(testCaseFilePath);

        if (!File.Exists(testCaseFilePath))
        {
            throw new FileNotFoundException($"Test case file not found: {testCaseFilePath}");
        }

        string json = File.ReadAllText(testCaseFilePath);
        Console.WriteLine(json);
        return JsonSerializer.Deserialize<IEnumerable<TTestCase>>(json, _options)
            ?? throw new JsonException($"Failed to deserialize test cases from {testCaseFilePath}");
    }
}
