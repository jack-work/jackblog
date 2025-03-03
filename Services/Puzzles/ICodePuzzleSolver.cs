using JackBlog.Models;

namespace JackBlog.Services;

public interface ICodePuzzleSolver<TTestCase, TInput, TResult> where TTestCase : ITestCase<TInput, TResult>
{
    public TResult Solve(TTestCase testCase);
}


