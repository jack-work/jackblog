using System.Text.Json;
using System.Diagnostics;
using JackBlog.Services;

namespace JackBlog.Models;

public class PuzzleAggregator
{
    private readonly List<PuzzlePost> _blogPosts;
    private readonly IEnumerable<ICodePuzzleService> _puzzleServices;
    private readonly ILogger<PuzzleAggregator> _logger;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

    public PuzzleAggregator(
        IEnumerable<ICodePuzzleService> puzzleServices, 
        ILogger<PuzzleAggregator> logger)
    {
        _puzzleServices = puzzleServices;
        _logger = logger;

        _logger.LogInformation("BlogService initializing");

        _blogPosts = puzzleServices.Select(service => new PuzzlePost(() =>
            service?.PuzzleName is {} puzzleName ?
                TrySolvePuzzleByName(puzzleName) :
                throw new ArgumentException())
        {
            Id = service.PuzzleName,
            Title = service.PuzzleName,
            Content = service.PuzzleName,
            PublishedDate = DateTime.Now,
            Author = "Jack",
        }).ToList();

        _logger.LogInformation("BlogService initialized with {0} posts", _blogPosts.Count);
    }

    public IEnumerable<PuzzlePost> GetAllPosts()
    {
        _logger.LogInformation("Getting all blog posts");
        return _blogPosts.OrderByDescending(p => p.PublishedDate);
    }

    public PuzzlePost? GetPostById(string id, int? puzzleId = null)
    {
        _logger.LogInformation("Getting blog post by ID: {0}, PuzzleId: {1}", id, puzzleId);

        var stopwatch = Stopwatch.StartNew();
        var post = _blogPosts.FirstOrDefault(p => p.Id == id);

        if (post == null)
        {
            _logger.LogWarning("Blog post with ID {0} not found", id);
            return null;
        }

        stopwatch.Stop();
        _logger.LogInformation("Retrieved blog post {0} in {1}ms", id, stopwatch.ElapsedMilliseconds);

        return post;
    }

    private List<PuzzleSolution>? TrySolvePuzzleByName(string puzzleName)
    {
        if (_puzzleServices.FirstOrDefault(service => service.PuzzleName == puzzleName) is { } service)
        {
            _logger.LogDebug($"Loading puzzle solutions for puzzle {puzzleName}");
            var result = service.Solve().ToList();
            _logger.LogInformation($"Loaded {result.Count()} puzzle solutions");
            return result;
        }
        else return null;
    }

    public string GetPuzzleResults(string id)
    {
        string result;
        if (_puzzleServices.FirstOrDefault(service => service.PuzzleName == id) is {} service)
        {
            _logger.LogDebug("Loading puzzle solutions for SordidArrays post");
            result = JsonSerializer.Serialize(service.Solve().ToList(), _options);
            _logger.LogInformation("Loaded {0} puzzle solutions", result.Count());
        }
        else
        {
            result = "No puzzle found!";
        }
        return result;
    }
}
