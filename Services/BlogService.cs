using System.Text.Json;
using System.Diagnostics;
using JackBlog.Services;

namespace JackBlog.Models;

public class BlogService
{
    private readonly List<BlogPost> _blogPosts;
    private readonly CodePuzzleService _sordidArraysService;
    private readonly ILogger<BlogService> _logger;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

    public BlogService(CodePuzzleService sordidArraysService, ILogger<BlogService> logger)
    {
        _sordidArraysService = sordidArraysService;
        _logger = logger;

        _logger.LogInformation("BlogService initializing");

        _blogPosts = [
            new BlogPost
            {
                Id = "SordidArrays",
                Title = "SordidArrays",
                Content = "A solution to finding the median of two sorted arrays.",
                PublishedDate = DateTime.Now,
                Author = "Jack",
                PuzzleSolutions = null // We'll load these on demand
            }
        ];

        _logger.LogInformation("BlogService initialized with {0} posts", _blogPosts.Count);
    }

    private PuzzleSolution CreatePuzzlePost<TInput, TResult>(
        IResolvedTestCase<TInput, TResult> results
    ) =>
        new PuzzleSolution
        {
            Description = results.Description,
            Input = JsonSerializer.Serialize(results.Input, _options),
            Expected = results?.Expected?.ToString() ?? "",
            Actual = results?.Actual?.ToString() ?? "",
        };

    public IEnumerable<BlogPost> GetAllPosts()
    {
        _logger.LogInformation("Getting all blog posts");
        return _blogPosts.OrderByDescending(p => p.PublishedDate);
    }

    public BlogPost? GetPostById(string id, int? puzzleId = null)
    {
        _logger.LogInformation("Getting blog post by ID: {0}, PuzzleId: {1}", id, puzzleId);

        var stopwatch = Stopwatch.StartNew();
        var post = _blogPosts.FirstOrDefault(p => p.Id == id);

        if (post == null)
        {
            _logger.LogWarning("Blog post with ID {0} not found", id);
            return null;
        }

        if (post.Id == "SordidArrays")
        {
            _logger.LogDebug("Loading puzzle solutions for SordidArrays post");
            // Lazily load puzzle solutions with optional filtering
            post.PuzzleSolutions = _sordidArraysService
                .Solve(puzzleId)
                .Select(CreatePuzzlePost)
                .ToList();

            _logger.LogInformation("Loaded {0} puzzle solutions", post.PuzzleSolutions.Count());
        }

        stopwatch.Stop();
        _logger.LogInformation("Retrieved blog post {0} in {1}ms", id, stopwatch.ElapsedMilliseconds);

        return post;
    }
}
