using JackBlog.Models;
public class ResolvedTestCase<TInput, TResult> : IResolvedTestCase<TInput, TResult>
{
    public string? Id { get; init; }
    public required string Description { get; init; }
    public required TInput Input { get; init; }
    public required TResult Expected { get; init; }
    public required TResult Actual { get; init; }
}
