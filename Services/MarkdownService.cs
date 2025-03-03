using Markdig;
using Microsoft.AspNetCore.Html;

namespace JackBlog.Services;

public class MarkdownService
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdownService()
    {
        // Configure Markdig with the standard extensions
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
    }

    public HtmlString RenderMarkdown(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return new HtmlString(string.Empty);

        string html = Markdown.ToHtml(markdown, _pipeline);
        return new HtmlString(html);
    }
}