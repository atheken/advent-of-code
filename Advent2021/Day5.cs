using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Advent2021;


public class Day5
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day5(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private record Point(int X, int Y);
    
    private class Line
    {
        private static readonly Regex LineParser = new ("(?<sX>[\\d]+),(?<sY>[\\d]+) -> (?<eX>[\\d]+),(?<eY>[\\d]+)", 
            RegexOptions.ExplicitCapture);
        
        public Point StartingPoint { get; set; }
        public Point EndingPoint { get; set; }

        public IEnumerable<Point> PointsOnLine()
        {
            if (IsHorizontal)
            {
                var start = Math.Min(StartingPoint.X, EndingPoint.X);
                var end = Math.Max(StartingPoint.X, EndingPoint.X);
                while (start <= end)
                {
                    yield return StartingPoint with {X = start};
                    start++;
                }
            }
            else if(IsVertical)
            {
                var start = Math.Min(StartingPoint.Y, EndingPoint.Y);
                var end = Math.Max(StartingPoint.Y, EndingPoint.Y);
                
                while (start <= end)
                {
                    yield return StartingPoint with {Y = start};
                    start++;
                }
            }
            else
            {
                var left = StartingPoint;
                var right = EndingPoint;
                if (StartingPoint.X > EndingPoint.X)
                {
                    left = EndingPoint;
                    right = StartingPoint;
                }

                var yIncrement = left.Y < right.Y ? 1 : -1;
                var row = 0;
                while (left.X + row <= right.X)
                {
                    yield return new Point(left.X + row, left.Y + row * yIncrement);
                    row++;
                }
            }
        }

        public bool IsHorizontal => StartingPoint.Y == EndingPoint.Y;
        public bool IsVertical => StartingPoint.X == EndingPoint.X; 

        public Line(string endpoints)
        {
            var parsed = LineParser.Match(endpoints);
            StartingPoint = new Point(int.Parse(parsed.Groups["sX"].Value), int.Parse(parsed.Groups["sY"].Value));
            EndingPoint = new Point(int.Parse(parsed.Groups["eX"].Value), int.Parse(parsed.Groups["eY"].Value));
        }
    }

    
    
    [Fact]
    public void Puzzle1()
    {
        var candidateLines = "day5.txt".ReadInputLines()
            .Select(k => new Line(k)).Where(k => k.IsHorizontal || k.IsVertical)
            .ToArray();

        var overlappingPoints = 
            candidateLines.SelectMany(l => l.PointsOnLine())
                .GroupBy(k => k).Count(k => k.Count() > 1);
        
        _testOutputHelper.WriteLine(overlappingPoints.ToString());
    }
    
    [Fact]
    public void Puzzle2()
    {
        var candidateLines = "day5.txt".ReadInputLines()
            .Select(k => new Line(k))
            .ToArray();

        var overlappingPoints = 
            candidateLines.SelectMany(l => l.PointsOnLine())
                .GroupBy(k => k).Count(k => k.Count() > 1);
        
        _testOutputHelper.WriteLine(overlappingPoints.ToString());
    }
}