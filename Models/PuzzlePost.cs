namespace JackBlog.Models;

public record PuzzlePost(Func<IEnumerable<PuzzleSolution>?>? solvePuzzle = null)
{
    public required string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string Author { get; set; } = string.Empty;
    public IEnumerable<PuzzleSolution>? PuzzleSolutions => _lazyPuzzleSlns.Value;
    private Lazy<IEnumerable<PuzzleSolution>?> _lazyPuzzleSlns = new (() => solvePuzzle?.Invoke());
}

