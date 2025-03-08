namespace JackBlog.Services;

public interface ICodePuzzleSolver<TInput, TResult>
{
    public TResult Solve(TInput testCase);
}


