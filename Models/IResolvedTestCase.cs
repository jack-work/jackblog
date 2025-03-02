namespace JackBlog.Models;

public interface IResolvedTestCase<TInput, TExpected> : ITestCase<TInput, TExpected>
{
  public TExpected Actual { get; init; }
}
