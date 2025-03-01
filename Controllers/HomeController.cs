using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JackBlog.Models;

namespace JackBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlogService _blogService;

    public HomeController(ILogger<HomeController> logger, BlogService blogService)
    {
        _logger = logger;
        _blogService = blogService;
    }

    public IActionResult Index()
    {
        var posts = _blogService.GetAllPosts();
        return View(posts);
    }

    public IActionResult Details(int id)
    {
        var post = _blogService.GetPostById(id);
        if (post == null)
        {
            return NotFound();
        }
        return View(post);
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
