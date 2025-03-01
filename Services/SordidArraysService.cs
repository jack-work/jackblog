using JackBlog.Models;

namespace JackBlog.Services;

using IntArray = IEnumerable<int>;

internal class SordidArraysService(
    [FromKeyedServices("SordidArrays")]
        ITestCaseProvider<SordidArraysTestCase, SordidArraysInput, IntArray> testCaseProvider
) : ICodePuzzleService<SordidArraysTestCase, SordidArraysInput, IntArray>
{
    public IEnumerable<IResolvedTestCase<SordidArraysInput, IntArray>> Solve()
    {
      return testCaseProvider.GetTestCases().Select(test => new ResolvedSordidArraysTestCase {
          Input = test.Input,
          Description = test.Description,
          Expected = test.Expected,
          Actual = []
      });
    }
}
