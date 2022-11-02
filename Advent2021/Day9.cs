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

    [Fact]
    public void Puzzle1()
    {
        var grid = new Grid("day9.txt".ReadInputLines());
        _output.WriteObject(grid.FindLowPoints().Sum(k => k.Value + 1));
    }

    [Fact]
    public void Puzzle2()
    {
        var grid = new Grid("day9.txt".ReadInputLines());
        var largestBasins = grid.GetBasins().OrderByDescending(k => k.Size).Take(3);
        var size = largestBasins.Aggregate(1, (seed, current) => seed * current.Size);
        _output.WriteObject(size);
    }

    private class Grid
    {
        public Grid(IEnumerable<string> lines)
        {
            Points = lines.Select(l => l.Select(f => int.Parse(f.ToString())).ToArray()).ToArray();
        }

        private int[][] Points { get; }

        private Basin ExpandBasin(Point searchPoint, Basin? existingBasin = null)
        {
            existingBasin ??= new Basin();

            if (existingBasin.AddPoint(searchPoint))
            {
                foreach (var pt in GetNeighbors(searchPoint))
                {
                    existingBasin = ExpandBasin(pt, existingBasin);
                }
            }

            return existingBasin;
        }

        private IEnumerable<Point> GetNeighbors(Point point)
        {
            if (point.Row - 1 >= 0)
            {
                yield return new Point(point.Row - 1, point.Column, Points[point.Row - 1][point.Column]);
            }

            if (point.Row + 1 <= Points.Length - 1)
            {
                yield return new Point(point.Row + 1, point.Column, Points[point.Row + 1][point.Column]);
            }

            if (point.Column - 1 >= 0)
            {
                yield return new Point(point.Row, point.Column - 1, Points[point.Row][point.Column - 1]);
            }

            if (point.Column + 1 <= Points[point.Row].Length - 1)
            {
                yield return new Point(point.Row, point.Column + 1, Points[point.Row][point.Column + 1]);
            }
        }

        public IEnumerable<Basin> GetBasins() =>
            FindLowPoints().Select(k => ExpandBasin(k))
                .Where(k => k.IsValidBasin);

        public IEnumerable<Point> FindLowPoints()
        {
            foreach (var (row, rowValues) in Points.Each())
            {
                foreach (var (column, val) in rowValues.Each())
                {
                    var subject = new Point(row, column, val);

                    // if the value before this one, and after this one in this line,
                    // and the value above and below this one, are all > val, this point
                    // is the lowest.
                    if (GetNeighbors(subject).All(k => k.Value > subject.Value))
                    {
                        yield return subject;
                    }
                }
            }
        }

        public record Point(int Row, int Column, int Value);

        public class Basin
        {
            private readonly HashSet<Point> _points = new();

            /// <summary>
            /// the basin is only valid if there is exactly one point at the minimum.
            /// </summary>
            public bool IsValidBasin => _points.Count(f => f.Value == _points.Min(k => k.Value)) == 1;

            /// <summary>
            /// The size of this basin.
            /// </summary>
            public int Size => _points.Count;

            public bool AddPoint(Point pt)
            {
                if (pt.Value < 9)
                {
                    return _points.Add(pt);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}