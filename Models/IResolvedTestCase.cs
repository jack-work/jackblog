namespace JackBlog.Models;

internal interface IResolvedTestCase<TInput, TExpected> : ITestCase<TInput, TExpected>
{
  public TExpected Actual { get; init; }
}
