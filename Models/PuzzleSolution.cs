namespace JackBlog.Models;

public class PuzzleSolution
{
    public required string Description { get; init; }
    public required string Input { get; init; }
    public required string Expected { get; init; }
    public required string Actual { get; init; }
}


