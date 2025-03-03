using JackBlog.Models;

namespace JackBlog.Services;

public interface ICodePuzzleService
{
    public string PuzzleName { get; }
    public IEnumerable<PuzzleSolution> Solve(int? specificIndex = null);
}
