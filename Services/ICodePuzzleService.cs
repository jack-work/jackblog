using JackBlog.Models;

namespace JackBlog.Services;

internal interface ICodePuzzleService<TTestCase, TInput, TExpected> where TTestCase : ITestCase<TInput, TExpected>
{
  public IEnumerable<IResolvedTestCase<TInput, TExpected>> Solve();
}
