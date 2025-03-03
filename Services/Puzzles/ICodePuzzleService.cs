using JackBlog.Models;

namespace JackBlog.Services;

public interface ICodePuzzleService
{
    public string PuzzleName { get; }
    public string PuzzleDescription { get; }
    public IEnumerable<PuzzleSolution> Solve(int? specificIndex = null);
}
