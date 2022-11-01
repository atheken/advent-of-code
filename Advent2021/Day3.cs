using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day3
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day3(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Puzzle1()
    {
        var values = "day3.txt".ReadInputLines()
            .Select(i => Convert.ToInt32(i, 2)).ToList();


        var bits = new Stack<int>();

        while (values.Sum(k => k) > 0)
        {
            var lowOrder = values.ToLookup(k => k & 1);
            bits.Push(lowOrder[0].Count() > lowOrder[1].Count() ? 0 : 1);
            values = values.Select(k => k >> 1).ToList();
        }

        var gamma = 0;
        var epsilon = 0;
        var shift = false;
        while (bits.TryPop(out var bit))
        {
            if (shift)
            {
                gamma <<= 1;
                epsilon <<= 1;
            }

            gamma |= bit;
            epsilon |= bit == 0 ? 1 : 0;

            shift = true;
        }

        _testOutputHelper.WriteLine((gamma * epsilon).ToString());
    }

    [Fact]
    public void Puzzle2()
    {
        var allValues = "day3.txt".ReadInputLines()
            .Select(i => Convert.ToInt32(i, 2)).ToArray();

        // make a copy to filter for least common.
        var co2values = allValues.ToArray();
        var oxygenValues = allValues.ToArray();

        var filter = 1 << 11;

        while (filter > 0)
        {
            // if zero is the most common bit, and the filter, then test for == 0

            // if zero is less common, and the filter, then test for > 0.
            if (co2values.Count() > 1)
            {
                var co2Group = co2values.ToLookup(k => (k & filter) > 0);
                co2values = (co2Group[false].Count() > co2Group[true].Count()
                    ? co2Group[false]
                    : co2Group[true]).ToArray();
            }

            if (oxygenValues.Count() > 1)
            {
                var oxygenGroup = oxygenValues.ToLookup(k => (k & filter) > 0);
                oxygenValues = (oxygenGroup[false].Count() > oxygenGroup[true].Count()
                    ? oxygenGroup[true]
                    : oxygenGroup[false]).ToArray();
            }

            filter >>= 1;
        }

        _testOutputHelper.WriteLine((co2values.First() * oxygenValues.First()) .ToString());
    }
}