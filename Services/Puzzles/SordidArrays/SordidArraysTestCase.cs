namespace JackBlog.Models;

public class SordidArraysTestCase : ITestCase<SordidArraysInput, double>
{
    public string? Id { get; set; }
    public required string Description { get; init; }
    public required SordidArraysInput Input { get; init; }
    public required double Expected { get; init; }
}

