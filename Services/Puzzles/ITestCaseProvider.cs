using JackBlog.Models;

namespace JackBlog.Services;

public interface ITestCaseProvider
{
  public IEnumerable<TTestCase> GetTestCases<TTestCase, TInput, TResult>(string testName) where TTestCase : ITestCase<TInput, TResult>;
}


