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

        public int Shift()
        {
            var eastMoves = new List<Action>();
            
            //returns number of elements that shifted.
            foreach (var row in _grid)
            {
                foreach (var (colIdx, element) in row.Each())
                {
                    if (element == PointDisposition.East)
                    {
                        var next = (colIdx + 1) % _width;
                        if (row[next] == PointDisposition.Empty)
                        {
                            eastMoves.Add(() =>
                            {
                                row[next] = element;
                                row[colIdx] = PointDisposition.Empty;
                            });
                        }
                    }
                }
            }

            eastMoves.ForEach(k=>k());

            var southMoves = new List<Action>();
            
            foreach (var (rowIdx, row) in _grid.Each())
            {
                foreach (var (colIdx, element) in row.Each())
                {
                    if (element == PointDisposition.South)
                    {
                        var next = (rowIdx + 1) % _height;
                        if (_grid[next][colIdx] == PointDisposition.Empty)
                        {
                            southMoves.Add(() =>
                            {
                                _grid[next][colIdx] = element;
                                row[colIdx] = PointDisposition.Empty;    
                            });
                        }
                    }
                }
            }
            
            southMoves.ForEach(k=> k());
            return eastMoves.Count + southMoves.Count;
        }
    }
    
    [Fact]
    public void Puzzle1()
    {
        var g = new Grid("day25.txt".ReadInputLines());
        var moves = 1;
        while (g.Shift() > 0)
        {
            moves++;
        }
        _output.WriteObject(moves);
    }
}