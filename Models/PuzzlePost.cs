namespace JackBlog.Models;

public record PuzzlePost
{
    public required string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string Author { get; set; } = string.Empty;
    public IEnumerable<PuzzleSolution>? PuzzleSolutions { get; init; }
}

