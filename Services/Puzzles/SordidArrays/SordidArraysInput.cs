namespace JackBlog.Models;

public sealed class SordidArraysInput
{
  public required IEnumerable<int> Left { get; init; }
  public required IEnumerable<int> Right { get; init; }
}

