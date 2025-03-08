using JackBlog.Models;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace JackBlog.Services;

public class CodePuzzleService<TTestCase, TInput, TResult> : ICodePuzzleService where TTestCase : ITestCase<TInput, TResult>
{
    private readonly ITestCaseProvider _testCaseProvider;
    private readonly ICodePuzzleSolver<TInput, TResult> _solver;
    private readonly ILogger<CodePuzzleService<TTestCase, TInput, TResult>> _logger;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };
    private readonly PuzzleSettings _puzzleSettings;

    public CodePuzzleService(
        string puzzleName,
        ICodePuzzleSolver<TInput, TResult> solver,
        ITestCaseProvider testCaseProvider,
        ILogger<CodePuzzleService<TTestCase, TInput, TResult>> logger,
        IOptions<PuzzleSettings> puzzleSettings)
    {
        PuzzleName = puzzleName;
        _testCaseProvider = testCaseProvider;
        _solver = solver;
        _logger = logger;
        _puzzleSettings = puzzleSettings.Value;
        PuzzleDescription = _testCaseProvider.GetPuzzleDescription(puzzleName);
        _logger.LogInformation($"{PuzzleName} initialized (RegenerateTestCaseSolutions: {_puzzleSettings.RegenerateTestCaseSolutions})");
    }

    public string PuzzleName { get; }
    public string PuzzleDescription { get; }
    public IEnumerable<PuzzleSolution> Solve(int? specificIndex)
    {
        return SolvePuzzle(specificIndex).Select(CreatePuzzlePost);
    }

    private PuzzleSolution CreatePuzzlePost(
        IResolvedTestCase<TInput, TResult> results
    ) =>
        new PuzzleSolution
        {
            Description = results.Description,
            Input = JsonSerializer.Serialize(results.Input, _options),
            Expected = results?.Expected?.ToString() ?? "",
            Actual = results?.Actual?.ToString() ?? "",
        };

    IEnumerable<IResolvedTestCase<TInput, TResult>> SolvePuzzle(int? specificIndex)
    {
        _logger.LogInformation("{0}", 0 / 2.0);
        _logger.LogInformation("----running test for {specificIndex}----", specificIndex);
        _logger.LogInformation("Solving {PuzzleName} puzzle with specificIndex: {SpecificIndex}", PuzzleName, specificIndex);
        var stopwatch = Stopwatch.StartNew();

        var testCases = _testCaseProvider.GetTestCases<TTestCase, TInput, TResult>(PuzzleName).ToList();
        _logger.LogDebug("Loaded {TestCaseCount} test cases", testCases.Count);

        IEnumerable<IResolvedTestCase<TInput, TResult>> results;

        if (specificIndex.HasValue && specificIndex.Value >= 0 && specificIndex.Value < testCases.Count)
        {
            _logger.LogDebug("Solving specific test case at index {Index}", specificIndex.Value);
            // Only solve the specific test case at the given index
            var testCase = testCases[specificIndex.Value];
            results = new List<IResolvedTestCase<TInput, TResult>>
            {
                new ResolvedTestCase<TInput, TResult>
                {
                    Id = specificIndex.Value.ToString(),
                    Input = testCase.Input,
                    Description = testCase.Description,
                    Expected = testCase.Expected,
                    Actual = _solver.Solve(testCase.Input),
                }
            };
        }
        else
        {
            _logger.LogDebug("Solving all test cases");
            // Solve all test cases
            results = testCases
                .Select((test, index) => new ResolvedTestCase<TInput, TResult>
                {
                    Id = index.ToString(),
                    Input = test.Input,
                    Description = test.Description,
                    Expected = test.Expected,
                    Actual = _solver.Solve(test.Input),
                })
                .ToList();
        }

        stopwatch.Stop();
        _logger.LogInformation("Solved {ResultCount} test cases in {ElapsedMs}ms", results.Count(), stopwatch.ElapsedMilliseconds);
        _logger.LogInformation("---------------------------------------------------------------");

        return results;
    }
}

