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
        var values = "puzzle3.txt".ReadInputLines()
            .Select(i => Convert.ToInt32(i, 2)).ToList();
        

        var bits = new Stack<int>();
        
        while (values.Sum(k=>k) > 0)
        {
            var lowOrder = values.ToLookup(k =>  k & 1);
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
        var allValues = "puzzle3.txt".ReadInputLines()
            .Select(i => Convert.ToInt32(i, 2)).ToList();

        // make a copy to filter for least common.
        var co2values = new List<int>(allValues);
        var oxygenValues = new List<int>(allValues);

        var filter = 1 << 11;

        while (filter > 0)
        {
            

            filter >>= 1;
        }
        
        _testOutputHelper.WriteLine((false).ToString());
    }
}