using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day9
{
    private readonly ITestOutputHelper _output;

    public Day9(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    private class Level
    {
        private int[] Points { get; }
        
        public Level(string line, Level? previousLevel)
        {
            Points = line.Select(f => int.Parse(f.ToString())).ToArray();
            if (previousLevel != null)
            {
                previousLevel.NextLevel = this;
            }
            PreviousLevel = previousLevel;

        }

        private int this[int index]
        {
            get
            {
                if (index < 0 || index > Points.Length - 1)
                {
                    return 9;
                }
                else
                {
                    return Points[index];
                }
            }
        }

        public int LevelCount => (PreviousLevel?.LevelCount ?? 0) + 1;

        public IEnumerable<int> FindLowPoints()
        {
            var retval = new List<int>();
            foreach (var (i, val) in Points.Each())
            {
                // if the value before this one, and after this one in this line,
                // and the value above and below this one, are all > val, this point
                // is the lowest.
                if (this[i - 1] > val && 
                    this[i + 1] > val && 
                    (PreviousLevel?[i] ?? 9) > val &&
                    (NextLevel?[i] ?? 9) > val)
                {
                    retval.Add(val + 1);
                }
            }

            return retval.Concat(PreviousLevel?.FindLowPoints() ?? new int[0]);

        }

        private Level? PreviousLevel { get; }
        private Level? NextLevel { get; set; }
    }
    
    [Fact]
    public void Puzzle1()
    {
        Level? currentLevel = null;
        foreach (var line in "day9.txt".ReadInputLines())
        {
            var parsedLevel = new Level(line, currentLevel);
            currentLevel = parsedLevel;
        }

        var levelCount = currentLevel.LevelCount;
        _output.WriteLine(currentLevel!.FindLowPoints().Sum().ToString());
    }
}