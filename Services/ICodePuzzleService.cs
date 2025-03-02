using JackBlog.Models;

namespace JackBlog.Services;

public interface ICodePuzzleService<TTestCase, TInput, TExpected> where TTestCase : ITestCase<TInput, TExpected>
{
  public IEnumerable<IResolvedTestCase<TInput, TExpected>> Solve(int? specificIndex = null);
}
