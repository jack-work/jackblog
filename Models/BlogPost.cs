namespace JackBlog.Models;

public class PuzzleSolution
{
    public required string Description { get; init; }
    public required string Input { get; init; }
    public required string Expected { get; init; }
    public required string Actual { get; init; }
}

public class BlogPost
{
    public required string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string Author { get; set; } = string.Empty;
    public IEnumerable<PuzzleSolution>? PuzzleSolutions { get; set; }
}

