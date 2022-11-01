using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day6
{
    private ITestOutputHelper _output;

    public Day6(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    private class Latternfish
    {
        private static readonly int GESTATION_CYCLE = 6;
        private int _clock;
        
        public Latternfish(int startingClock = 8)
        {
            _clock = startingClock;
        }

        public bool GestateAndYield(out Latternfish? newFish)
        {
            _clock--;
            if (_clock < 0)
            {
                _clock = GESTATION_CYCLE;
                newFish = new Latternfish();
                return true;
            }

            newFish = null;
            return false;
        }
        
    }
    
    [Fact]
    public void Puzzle1()
    {
        var fish = "day6.txt".ReadInputLines().First().Split(',')
            .Select(l => new Latternfish(int.Parse(l))).ToList();

        var generation = 0;
        while(generation < 80)
        {
            foreach (var f in fish.ToArray())
            {
                if (f.GestateAndYield(out var l))
                {
                    fish.Add(l!);
                }
            }

            generation++;
        }
        _output.WriteLine(fish.Count.ToString());
    }

    [Fact]
    public void Puzzle2()
    {
        var fish = "day6.txt".ReadInputLines().First().Split(',')
            .Select(int.Parse).GroupBy(k => k)
            .ToDictionary(k => k.Key, v => v.LongCount());

        var generation = 0;
        while(generation < 256)
        {
            var newFish = new Dictionary<int, long>();
            for (var i = 0; i < 9; i++)
            {
                if (fish.ContainsKey(i))
                {
                    if (i == 0)
                    {
                        newFish[6] = fish[i];
                        newFish[8] = fish[i];
                    }
                    else if(newFish.ContainsKey(i - 1))
                    {
                        newFish[i - 1] += fish[i];
                    }
                    else
                    {
                        newFish[i - 1] = fish[i];
                    }
                }
            }

            fish = newFish;
            generation++;
        }
        _output.WriteLine(fish.Sum(k=>k.Value).ToString());
    }

}