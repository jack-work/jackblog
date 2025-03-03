using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JackBlog.Models;

namespace JackBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PuzzleAggregator _puzzleAggregator;

    public HomeController(ILogger<HomeController> logger, PuzzleAggregator puzzleAggregator)
    {
        _logger = logger;
        _puzzleAggregator = puzzleAggregator;
    }

    public IActionResult Index()
    {
        var posts = _puzzleAggregator.GetAllPosts();
        return View(posts);
    }

    public IActionResult Details(string id, string? puzzleId)
    {
        int? puzzleIndex = null;
        
        if (!string.IsNullOrEmpty(puzzleId) && int.TryParse(puzzleId, out int index))
        {
            puzzleIndex = index;
            ViewBag.PuzzleId = puzzleId;
        }
        
        var post = _puzzleAggregator.GetPostById(id, puzzleIndex);
        if (post == null)
        {
            return NotFound();
        }
        
        return View(post);
    }

    public IActionResult JsonResults(string id)
    {
        return Content(_puzzleAggregator.GetPuzzleResults(id), "text/plain");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
