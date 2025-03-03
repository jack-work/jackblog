namespace JackBlog.Models;

public class TestCase<TInput, TResult> : ITestCase<TInput, TResult>
{
    public required string Description { get ; init; }
    public required TInput Input { get ; init; }
    public required TResult Expected { get ; init; }
}

