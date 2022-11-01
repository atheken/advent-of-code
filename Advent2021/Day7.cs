using System.Collections.Immutable;
using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day7
{
    private readonly ITestOutputHelper _output;

    public Day7(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    [Fact]
    public void Puzzle1()
    {
        var subs = "day7.txt".ReadSingleIntCsv();
        var grouped = subs.GroupBy(k => k).ToDictionary(k => k.Key, v => v.Count());
        var min = int.MaxValue;

        foreach (var element in grouped)
        {
           var sum = grouped.Sum(k=> Math.Abs(element.Key - k.Key) * k.Value);
           min = Math.Min(sum, min);
        }
        
        _output.WriteLine(min.ToString());
    }
    
    [Fact]
    public void Puzzle2()
    {
        var subs = "day7.txt".ReadSingleIntCsv();
        
        var costs = GetFuelCost(subs.Max() - subs.Min());
        var grouped = subs.GroupBy(k => k).ToDictionary(k => k.Key, v => v.LongCount());
        var point = (int)Math.Abs(subs.Average());
        
        var requiredFuel = 0L;
        foreach (var (moverLocation, count) in grouped)
        {
            var distance = Math.Abs(point - moverLocation);
            requiredFuel += costs[distance] * count;
        }
    
        _output.WriteLine(requiredFuel.ToString());
    }

    private Dictionary<int, long> GetFuelCost(int maxCost)
    {
        var retval = new Dictionary<int, long>();
        var previous = 0;
        for (var i = 0; i <= maxCost; i++)
        {
            var cost = i + previous;
            retval[i] = cost;
            previous = cost;
        }

        return retval;
    }
}