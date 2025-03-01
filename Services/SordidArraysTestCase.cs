namespace JackBlog.Models;

internal class SordidArraysTestCase : ITestCase<SordidArraysInput, IEnumerable<int>>
{
    public required string Description { get; init; }
    public required SordidArraysInput Input { get; init; }
    public required IEnumerable<int> Expected { get; init; }
}


internal class ResolvedSordidArraysTestCase : IResolvedTestCase<SordidArraysInput, IEnumerable<int>>
{
    public required string Description { get; init; }
    public required SordidArraysInput Input { get; init; }
    public required IEnumerable<int> Expected { get; init; }
    public required IEnumerable<int> Actual { get; init; }
}
