using JackBlog.Models;
public class ResolvedSordidArraysTestCase : IResolvedTestCase<SordidArraysInput, double>
{
    public string? Id { get; init; }
    public required string Description { get; init; }
    public required SordidArraysInput Input { get; init; }
    public required double Expected { get; init; }
    public required double Actual { get; init; }
}
