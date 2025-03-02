using JackBlog.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace JackBlog.Services;

public class CodePuzzleService : ICodePuzzleService<SordidArraysTestCase, SordidArraysInput, double>
{
    private readonly ITestCaseProvider<SordidArraysTestCase, SordidArraysInput, double> _testCaseProvider;
    private readonly ILogger<CodePuzzleService> _logger;

    public CodePuzzleService(
        [FromKeyedServices("SordidArrays")] ITestCaseProvider<SordidArraysTestCase, SordidArraysInput, double> testCaseProvider,
        ILogger<CodePuzzleService> logger)
    {
        _testCaseProvider = testCaseProvider;
        _logger = logger;
        _logger.LogInformation("SordidArraysService initialized");
    }

    public IEnumerable<IResolvedTestCase<SordidArraysInput, double>> Solve(int? specificIndex = null)
    {
        _logger.LogInformation("{0}", 0 / 2.0);
        _logger.LogInformation("----running test for {specificIndex}----", specificIndex);
        _logger.LogInformation("Solving SordidArrays puzzle with specificIndex: {SpecificIndex}", specificIndex);
        var stopwatch = Stopwatch.StartNew();

        var testCases = _testCaseProvider.GetTestCases().ToList();
        _logger.LogDebug("Loaded {TestCaseCount} test cases", testCases.Count);

        IEnumerable<IResolvedTestCase<SordidArraysInput, double>> results;

        if (specificIndex.HasValue && specificIndex.Value >= 0 && specificIndex.Value < testCases.Count)
        {
            _logger.LogDebug("Solving specific test case at index {Index}", specificIndex.Value);
            // Only solve the specific test case at the given index
            var testCase = testCases[specificIndex.Value];
            results = new List<IResolvedTestCase<SordidArraysInput, double>>
            {
                new ResolvedSordidArraysTestCase
                {
                    Id = specificIndex.Value.ToString(),
                    Input = testCase.Input,
                    Description = testCase.Description,
                    Expected = testCase.Expected,
                    Actual = SolveTestCase(testCase),
                }
            };
        }
        else
        {
            _logger.LogDebug("Solving all test cases");
            // Solve all test cases
            results = testCases
                .Select((test, index) => new ResolvedSordidArraysTestCase
                {
                    Id = index.ToString(),
                    Input = test.Input,
                    Description = test.Description,
                    Expected = test.Expected,
                    Actual = SolveTestCase(test),
                })
                .ToList();
        }

        stopwatch.Stop();
        _logger.LogInformation("Solved {ResultCount} test cases in {ElapsedMs}ms", results.Count(), stopwatch.ElapsedMilliseconds);
        _logger.LogInformation("---------------------------------------------------------------");

        return results;
    }

    private double SolveTestCase(SordidArraysTestCase test)
    {
        var sw = Stopwatch.StartNew();
        var result = Solve(test.Input.Left.ToArray(), test.Input.Right.ToArray());
        sw.Stop();

        _logger.LogDebug("Solved test case in {ElapsedMs}ms", sw.ElapsedMilliseconds);

        return result;
    }

    private int _lim = 1000;
    private double Solve(int[] left, int[] right)
    {
        // Special case where C# math causes result to evaluate to 0.
        if (left.Length == 0 && right.Length == 0) return 0;
        var lenL = left.Length;
        var lenR = right.Length;
        if (lenL < lenR) return Solve(right, left);

        var lenTotal = lenL + lenR;
        var half = lenTotal / 2;

        var sepL = lenL / 2;
        var sepR = half - sepL;
        for (var i = 0; i < _lim; i++)
        {
            var leftL = Get(left, sepL - 1);
            var leftR = Get(left, sepL);
            var rightL = Get(right, sepR - 1);
            var rightR = Get(right, sepR);
            ps(left, sepL, right, sepR);
            if (rightL > leftR)
            {
                sepL++;
                sepR--;
                continue;
            }
            if (leftL > rightR)
            {
                sepL--;
                sepR++;
                continue;
            }

            var minR = Math.Min(leftR, rightR);
            var maxL = Math.Max(leftL, rightL);
            return lenTotal % 2 != 0 ? minR : (minR + maxL) / 2.0;
        }

        return 0;
    }

    private int Get(int[] arr, int index) =>
        index < 0 ? Int32.MinValue :
        index >= arr.Length ? Int32.MaxValue :
        arr[index];

    private void ps2<T>(T sepL, T sepR,
        [CallerArgumentExpression(nameof(sepL))] string? sepLName = null,
        [CallerArgumentExpression(nameof(sepR))] string? sepRName = null
    )
    {
        _logger.l(JsonSerializer.Serialize(new Dictionary<string, T>
        {
        { sepLName ?? "var1", sepL },
        { sepRName ?? "var2" , sepR },
        }, new JsonSerializerOptions { WriteIndented = true }));
    }

    private void ps(int[] left, int sepL, int[] right, int sepR)
    {
        try
        {
            _logger.l(JsonSerializer.Serialize(new
            {
                left = string.Join(',', left[..sepL]) + '|' + string.Join(',', left[sepL..]),
                right = string.Join(',', right[..sepR]) + '|' + string.Join(',', right[sepR..]),
                // left = string.Join(',', left) + '|' + string.Join(',', left),
                // right = string.Join(',', right) + '|' + string.Join(',', right),
            }, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception e)
        {
            _logger.l(e.ToString());
            _logger.l(JsonSerializer.Serialize(new
            {
                sepL,
                sepR,
                left = string.Join(',', left) + '|' + string.Join(',', left),
                right = string.Join(',', right) + '|' + string.Join(',', right),
            }, new JsonSerializerOptions { WriteIndented = true }));
        }
    }

}

public static class Extensions
{
    public static void l<T>(this ILogger<T> logger, string content) => logger.LogInformation(content);
}
