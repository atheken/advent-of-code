using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day25
{
    private readonly ITestOutputHelper _output;

    public Day25(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    public enum PointDisposition
    {
        East = '>',
        South = 'v',
        Empty = '.'
    }


    private class Grid
    {
        private readonly PointDisposition[][] _grid;
        private readonly int _width;
        private readonly int _height;

        public Grid(IEnumerable<string> lines)
        {
            _grid = lines.Select(k => k.Select(c => (PointDisposition) c).ToArray()).ToArray();
            _width = _grid[0].Length;
            _height = _grid.Length;
        }

        private IEnumerable<Action> FindMoves(
            PointDisposition elementType,
            Func<int, int> findNextRow,
            Func<int, int> findNextColumn)
        {
            //locate all available move options.
            foreach (var (rowIdx, row) in _grid.Each())
            {
                foreach (var (colIdx, element) in row.Each())
                {
                    if (elementType == element)
                    {
                        var nextRowId = findNextRow(rowIdx);
                        var nextColId = findNextColumn(colIdx);
                        if (_grid[nextRowId][nextColId] == PointDisposition.Empty)
                        {
                            yield return () =>
                            {
                                _grid[nextRowId][nextColId] = element;
                                _grid[rowIdx][colIdx] = PointDisposition.Empty;
                            };
                        }
                    }
                }
            }
        }

        public int Shift()
        {
            //apply all east shifts.
            var eastMoves = FindMoves(PointDisposition.East, f => f, f => (f + 1) % _width).ToList();
            eastMoves.ForEach(k => k());

            var southMoves = FindMoves(PointDisposition.South, f => (f + 1) % _height, f => f).ToList();
            southMoves.ForEach(k => k());

            //return however many shifts where applied.
            return eastMoves.Count + southMoves.Count;
        }
    }

    [Fact]
    public void Puzzle1()
    {
        var g = new Grid("day25.txt".ReadInputLines());

        // we start at 1 for the return value, because the question asks
        // for the first generation where the grid is stable.
        var moves = 1;
        while (g.Shift() > 0)
        {
            moves++;
        }

        _output.WriteObject(moves);
    }
}