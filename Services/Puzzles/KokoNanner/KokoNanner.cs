namespace JackBlog.Services;

/// <remarks>
/// |Piles| <= NumHours
/// </remarks>
public record KokoNannerInput(int NumHours, int[] Piles);
public class KokoNannerSolver(ILogger<KokoNannerSolver> _logger) : ICodePuzzleSolver<KokoNannerInput, int>
{
    public int Solve(KokoNannerInput testCase)
    {
        var h = testCase.NumHours;
        var piles = testCase.Piles;
        var max = piles.Max();
        return BinarySearch(0, max, amount =>
        {
            Console.WriteLine(amount);
            if (amount == 0) return false;

            // Use long to track hours instead of tracking total nanners
            long totalHours = 0;
            foreach (var pile in piles)
            {
                // Calculate hours needed for this pile and add to total
                totalHours += (long)Math.Ceiling((double)pile / amount);

                // Early exit if we've already exceeded the hour limit
                if (totalHours > h)
                    return false;
            }

            return totalHours <= h;
        });
    }
    private int BinarySearch(int min, int max, Func<int, bool> decide)
    {
        var start = min + (max - min) / 2;
        for (var i = 0; i < 100; i++)
        {
            bool v = decide(start);
            // Console.WriteLine($"{v}, {min}, {start}, {max}");
            if (v) max = start;
            else min = start;
            if (min == max) return start;
            else if (min == max - 1)
            {
                return v ? start : max;
            }
            start = min + (max - min) / 2;
        }
        return 0;
    }
}
