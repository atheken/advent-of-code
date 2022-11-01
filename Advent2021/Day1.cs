using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Puzzle1()
    {
        var input = "day1.txt".ReadInputLines();
        int? previous = null;
        var increasingCount = 0;
        
        foreach (var i in input.Select(int.Parse))
        {
            if (previous.HasValue && i > previous)
            {
                increasingCount++;
            }
            previous = i;
        }
        _testOutputHelper.WriteLine(increasingCount.ToString());
    }
    
    [Fact]
    public void Puzzle2()
    {
        var input = "day1.txt".ReadInputLines();
        var window = new Queue<int>();
        int? previous = null;
        
        var increasingCount = 0;
        
        foreach (var i in input.Select(int.Parse))
        {
            if (window.Any())
            {
                previous = window.Sum();
                window.Enqueue(i);
                if (window.Count > 3)
                {
                    window.Dequeue();
                    var currentSum = window.Sum();
                    if (currentSum > previous)
                    {
                        increasingCount++;
                    }
                }
            }
            else
            {
                window.Enqueue(i);
            }
        }
        _testOutputHelper.WriteLine(increasingCount.ToString());
    }
    
}