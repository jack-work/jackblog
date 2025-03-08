using JackBlog.Models;
using System.Diagnostics;

namespace JackBlog.Services;

public class SordidArraysSolver(ILogger<SordidArraysSolver> _logger) : ICodePuzzleSolver<SordidArraysInput, double>
{
    public double Solve(SordidArraysInput test)
    {
        _logger.LogDebug("Solving SordidArrays");
        var sw = Stopwatch.StartNew();
        var result = Solve(test.Left.ToArray(), test.Right.ToArray());
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
}

