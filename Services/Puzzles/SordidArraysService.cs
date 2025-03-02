using JackBlog.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace JackBlog.Services;

public class SordidArraysService : ICodePuzzleService<SordidArraysTestCase, SordidArraysInput, double>
{
    private readonly ITestCaseProvider<SordidArraysTestCase, SordidArraysInput, double> _testCaseProvider;
    private readonly ILogger<SordidArraysService> _logger;

    public SordidArraysService(
        [FromKeyedServices("SordidArrays")] ITestCaseProvider<SordidArraysTestCase, SordidArraysInput, double> testCaseProvider,
        ILogger<SordidArraysService> logger)
    {
        _testCaseProvider = testCaseProvider;
        _logger = logger;
        _logger.LogInformation("SordidArraysService initialized");
    }

    public IEnumerable<IResolvedTestCase<SordidArraysInput, double>> Solve(int? specificIndex = null)
    {
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

    private double Solve(int[] nums1, int[] nums2)
    {
        _logger.LogDebug("Solving SordidArrays with arrays of length {Length1} and {Length2}", nums1.Length, nums2.Length);
        
        if (nums1.Length > nums2.Length) 
        {
            _logger.LogDebug("Swapping arrays to ensure nums1 is smaller");
            return Solve(nums2, nums1); // Ensure nums1 is smaller
        }
        
        int m = nums1.Length, n = nums2.Length;
        int left = 0, right = m, halfLen = (m + n + 1) / 2;
        
        _logger.LogTrace("Binary search starting with left={Left}, right={Right}, halfLen={HalfLen}", left, right, halfLen);
        
        int iterations = 0;
        while (left <= right) {
            iterations++;
            int i = (left + right) / 2;
            int j = halfLen - i;

            int nums1Left = (i == 0) ? int.MinValue : nums1[i - 1];
            int nums1Right = (i == m) ? int.MaxValue : nums1[i];
            int nums2Left = (j == 0) ? int.MinValue : nums2[j - 1];
            int nums2Right = (j == n) ? int.MaxValue : nums2[j];

            _logger.LogTrace("Iteration {Iteration}: i={I}, j={J}", iterations, i, j);
            
            if (nums1Left <= nums2Right && nums2Left <= nums1Right) {
                double result;
                if ((m + n) % 2 == 0)
                    result = (Math.Max(nums1Left, nums2Left) + Math.Min(nums1Right, nums2Right)) / 2.0;
                else
                    result = Math.Max(nums1Left, nums2Left);
                
                _logger.LogDebug("Found solution in {Iterations} iterations: {Result}", iterations, result);
                return result;
            } else if (nums1Left > nums2Right) {
                _logger.LogTrace("Adjusting right bound from {Right} to {NewRight}", right, i - 1);
                right = i - 1;
            } else {
                _logger.LogTrace("Adjusting left bound from {Left} to {NewLeft}", left, i + 1);
                left = i + 1;
            }
        }
        
        _logger.LogError("Failed to find median - input arrays may not be sorted");
        throw new ArgumentException("Input arrays are not sorted.");
    }
}
