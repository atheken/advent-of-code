using Advent2021.Model;
using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day11
{
    private readonly ITestOutputHelper _output;

    public Day11(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    private class Octopus
    {
        public int Energy { get; set; }
    }

    private int IncrementAndFlash(GenerationalGrid<Octopus> grid)
    {
        //increment all the energy levels.
        foreach (var a in grid.Cells())
        {
            a.Value.Energy++;
        }

        //now, flash all the cells
        var flashedOctopuses = new HashSet<GridCell<Octopus>>();
        var flashCount = 0;
        do
        {
            flashCount = flashedOctopuses.Count;
            // enumerate all the cells.
            foreach (var a in grid.Cells())
            {
                // if the energy value is greater than 9, and it hasn't been flashed,
                // flash it, and increment the energy in the adjacent cells.
                if (a.Value.Energy > 9 && !flashedOctopuses.Contains(a))
                {
                    flashedOctopuses.Add(a);
                    foreach (var p in a.AdjacentCells())
                    {
                        p.Value.Energy++;
                    }
                }
            }
        } while (flashCount != flashedOctopuses.Count);

        // reset the energy level for all octopuses that have flashed.
        foreach (var a in flashedOctopuses)
        {
            a.Value.Energy = 0;
        }

        return flashedOctopuses.Count;
    }

    [Fact]
    public void Puzzle1()
    {
        var grid = new GenerationalGrid<Octopus>("day11.txt".ReadInputLines(),
            f => f.Select(k => new Octopus {Energy = k.CharToInt()}));
        
        var flashes = Enumerable.Range(0, 100).Sum(_ => IncrementAndFlash(grid));
        _output.WriteObject(flashes);
    }
    
    [Fact]
    public void Puzzle2()
    {
        var grid = new GenerationalGrid<Octopus>("day11.txt".ReadInputLines(),
            f => f.Select(k => new Octopus {Energy = k.CharToInt()}));
        
        var step = 0;
        var flashes = 0;
        
        while (flashes != grid.CellCount)
        {
            flashes = IncrementAndFlash(grid);
            step++;
        }
        _output.WriteObject(step);
    }
}