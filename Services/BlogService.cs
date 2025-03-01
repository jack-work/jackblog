using JackBlog.Services;

namespace JackBlog.Models;

public class BlogService
{
    private readonly List<BlogPost> _blogPosts;

    // public BlogService(SordidArraysService sordidArraysService)
    public BlogService()
    {
        // Initialize with some static blog entries
        _blogPosts = new List<BlogPost>
        {
            new BlogPost
            {
                Id = 1,
                Title = "Getting Started with ASP.NET Core",
                Content =
                    "ASP.NET Core is a cross-platform, high-performance, open-source framework for building modern, cloud-based, Internet-connected applications.",
                PublishedDate = new DateTime(2023, 1, 15),
                Author = "Jack",
            },
            new BlogPost
            {
                Id = 2,
                Title = "Working with Entity Framework Core",
                Content =
                    "Entity Framework Core is a modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations.",
                PublishedDate = new DateTime(2023, 2, 10),
                Author = "Jack",
            },
            new BlogPost
            {
                Id = 3,
                Title = "Building RESTful APIs with ASP.NET Core",
                Content =
                    "Learn how to build RESTful APIs using ASP.NET Core and how to consume them using JavaScript or other client technologies.",
                PublishedDate = new DateTime(2023, 3, 5),
                Author = "Jack",
            },
        };
    }

    public IEnumerable<BlogPost> GetAllPosts()
    {
        return _blogPosts.OrderByDescending(p => p.PublishedDate);
    }

    public BlogPost? GetPostById(int id)
    {
        return _blogPosts.FirstOrDefault(p => p.Id == id);
    }
}
