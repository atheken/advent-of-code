namespace Advent2021.Model;

public record GridCell<T>(int Row, int Column, T Value, GenerationalGrid<T> Grid)
{
    public IEnumerable<GridCell<T>> AdjacentCells()
    {
        var results = new List<GridCell<T>?>();
        results.Add(Grid[Row -1, Column -1]);
        results.Add(Grid[Row -1, Column]);
        results.Add(Grid[Row -1, Column + 1]);
        results.Add(Grid[Row, Column -1]);
        results.Add(Grid[Row, Column]);
        results.Add(Grid[Row, Column + 1]);
        results.Add(Grid[Row + 1, Column -1]);
        results.Add(Grid[Row + 1, Column]);
        results.Add(Grid[Row + 1, Column + 1]);
        
        //find the cells that are in the parent grid that are "adjacent"
        return results.Where(k => k != null).Select(k=> k!);
    }
}

public class GenerationalGrid<T>
{
    private readonly T[][] _grid;

    public GenerationalGrid(IEnumerable<string> lines, Func<string, IEnumerable<T>> transform)
    {
        _grid = lines.Select(k => transform(k).ToArray()).ToArray();
    }

    public int CellCount => _grid.Length * _grid[0].Length;
    
    public GridCell<T>? this[int row,int column]
    {
        get
        {
            if (row >= 0 && row < _grid.Length && column >= 0 && column < _grid[0].Length)
            {
                return new(row, column, _grid[row][column], this);
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Applies the given function to each cell/row in a grid.
    /// </summary>
    public IEnumerable<GridCell<T>> Cells()
    {
        for (var row = 0; row < _grid.Length; row++)
        {
            for (var col = 0; col < _grid[row].Length; col++)
            {
                yield return new GridCell<T>(row, col, _grid[row][col], this);
            }
        }
    }
    
}