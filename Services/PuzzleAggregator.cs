using System.Diagnostics;
using System.Text.Json;
using JackBlog.Services;

namespace JackBlog.Models;

public class PuzzleAggregator
{
    private readonly IEnumerable<ICodePuzzleService> _puzzleServices;
    private readonly ILogger<PuzzleAggregator> _logger;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
    };

    public PuzzleAggregator(
        IEnumerable<ICodePuzzleService> puzzleServices,
        ILogger<PuzzleAggregator> logger
    )
    {
        _puzzleServices = puzzleServices;
        _logger = logger;

        _logger.LogInformation("BlogService initializing");
    }

    private List<PuzzlePost> GetPuzzlePosts(string? name = null, int? solutionId = null)
    {
        var services = name is not null ? _puzzleServices.Where(service => service.PuzzleName == name) : _puzzleServices;
        var result = services
            .Select(service => new PuzzlePost()
            {
                Id = service.PuzzleName,
                Title = service.PuzzleName,
                Content = service.PuzzleDescription,
                PublishedDate = DateTime.Now,
                Author = "Jack",
                PuzzleSolutions = TrySolvePuzzleByName(service.PuzzleName, solutionId),
            }).ToList();
        _logger.LogInformation("BlogService initialized with {0} posts", result.Count);
        return result;
    }

    public IEnumerable<PuzzlePost> GetAllPosts()
    {
        _logger.LogInformation("Getting all blog posts");
        return GetPuzzlePosts().OrderByDescending(p => p.PublishedDate);
    }

    public PuzzlePost? GetPostById(string puzzleName, int? solutionId = null)
    {
        _logger.LogInformation("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        _logger.LogInformation("Getting blog post by name: {0}, PuzzleId: {1}", puzzleName, solutionId);

        var stopwatch = Stopwatch.StartNew();
        var post = GetPuzzlePosts(puzzleName, solutionId);
        _logger.LogInformation("post:::: " + puzzleName);

        if (post == null)
        {
            _logger.LogWarning("Blog post with name {0} not found", puzzleName);
            return null;
        }

        stopwatch.Stop();
        _logger.LogInformation(
            "Retrieved blog post {0} in {1}ms",
            puzzleName,
            stopwatch.ElapsedMilliseconds
        );

        if (post.FirstOrDefault() is not {} returnValue)
        {
            _logger.LogError("No puzzle post found for the specified criteria");
            return null;
        }
        return returnValue;
    }

    private List<PuzzleSolution>? TrySolvePuzzleByName(string puzzleName, int? id = null)
    {
        if (
            _puzzleServices.FirstOrDefault(service => service.PuzzleName == puzzleName) is
            { } service
        )
        {
            _logger.LogDebug($"Loading puzzle solutions for puzzle {puzzleName}");
            var result = service.Solve(id).ToList();
            _logger.LogInformation($"Loaded {result.Count()} puzzle solutions");
            return result;
        }
        else
            return null;
    }

    public string GetPuzzleResults(string id)
    {
        string result;
        if (_puzzleServices.FirstOrDefault(service => service.PuzzleName == id) is { } service)
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
