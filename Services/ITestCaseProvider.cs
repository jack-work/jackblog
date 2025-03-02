using JackBlog.Models;

namespace JackBlog.Services;

public interface ITestCaseProvider<TTestCase, TInput, TResult> where TTestCase : ITestCase<TInput, TResult>
{
  public IEnumerable<TTestCase> GetTestCases();
}


