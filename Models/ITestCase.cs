namespace JackBlog.Models;
internal interface ITestCase<TInput, TExcepted>
{
    public string Description { get; init; }
    public TInput Input { get; init; }
    public TExcepted Expected { get; init; }
}
