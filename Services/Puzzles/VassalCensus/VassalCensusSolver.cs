using System.Diagnostics;
using System.Text.Json;

namespace JackBlog.Services;

// from duchy swears fealty toward to duchy
public record VassalCensusInput(int DuchyCount, int[][] FealtyRecord);

public class VassalCensusSolver(ILogger<ICodePuzzleSolver<VassalCensusInput, int>> logger)
    : ICodePuzzleSolver<VassalCensusInput, int>
{
    public int Solve(VassalCensusInput testCase)
    {
        logger.LogInformation("Starting to solve VassalCensus puzzle");
        var stopwatch = Stopwatch.StartNew();

        (int duchyCount, int[][] battles) = testCase;
        var domesday = new int[duchyCount];
        var lieges = Enumerable.Range(0, duchyCount).Select(i => i).ToArray();

        logger.LogDebug(
            "Processing {BattleCount} battles for {DuchyCount} duchies",
            battles.Length,
            duchyCount
        );

        foreach (var liege in battles)
        {
            if (liege is not [var duke1, var duke2])
                throw new Exception("a pox upon you");
            var lord1 = Find(lieges, duke1);
            var lord2 = Find(lieges, duke2);
            var rank1 = domesday[lord1];
            var rank2 = domesday[lord2];
            switch (rank1 - rank2)
            {
                case 0:
                    domesday[duke1]++;
                    lieges[lord2] = lord1;
                    break;
                case > 0:
                    lieges[lord2] = lord1;
                    break;
                default:
                    lieges[lord1] = lord2;
                    break;
            }
            l(
                new
                {
                    after = true,
                    d1 = lieges[duke1],
                    d2 = lieges[duke2],
                    l1 = lieges[lord1],
                    l2 = lieges[lord2],
                }
            );
        }

        // Assume every duchy is free, and decrement the counter when a king is found.
        var tally = 0;
        var coronations = new bool[duchyCount];
        for (var i = 0; i < duchyCount; i++)
        {
            var king = lieges[i];
            if (!coronations[king])
            {
                coronations[king] = true;
                tally++;
            }
        }

        l(
            new
            {
                duchyCount,
                lieges,
                domesday,
                coronations,
                tally,
            }
        );

        stopwatch.Stop();
        logger.LogInformation(
            "VassalCensus puzzle solved in {ElapsedMilliseconds}ms with result: {Result}",
            stopwatch.ElapsedMilliseconds,
            tally
        );

        return tally;
    }

    private int Find(int[] lieges, int duke)
    {
        var result = duke;
        while (lieges[result] != result)
            result = lieges[duke];
        lieges[duke] = result;
        return result;
    }

    public static void l(object input) =>
        Console.WriteLine(
            JsonSerializer.Serialize(input, new JsonSerializerOptions { WriteIndented = true })
        );
}
