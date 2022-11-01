using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day4
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day4(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private class BingoBoard
    {
        private readonly int[][] _board;
        private static readonly Regex Digits = new ("[\\d]+");
        
        public BingoBoard(Span<string> board)
        {
            _board = board.ToArray().Select(k=>Digits.Matches(k)
                .Select(f=> int.Parse(f.Value))
                .ToArray()).ToArray();
        }

        /// <summary>
        /// Returns if marking the board results in a row, column, or diagonal, being fully marked.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool MarkBoard(int value)
        {
            foreach (var row in _board)
            {
                for (var i = 0; i < row.Length; i++)
                {
                    if (row[i] == value)
                    {
                        row[i] = -1;
                        return IsBingo();
                    }
                }
            }

            return false;
        }

        private bool IsBingo()
        {
            // if the entire row is marked, it's a bingo.
            if (_board.Any(f => f.All(x => x == -1)))
            {
                return true;
            }
            else
            {
                // if a specific column is marked, it's a bingo.
                for (var i = 0; i < _board[0].Length; i++)
                {
                    if (_board.All(k => k[i] == -1))
                    {
                        return true;
                    }
                }

                //if the descending diagonal is set, bingo.
                for (var i = 0; i < _board.Length ; i++)
                {
                    if (_board[i][i] != -1)
                    {
                        break;
                    }else if (i == _board.Length)
                    {
                        return true;
                    }
                }

                //if the descending diagonal is set, bingo.
                for (var i = _board.Length - 1; i >= 0; i--)
                {
                    if (_board[i][i] != -1)
                    {
                        break;
                    }else if (i == 0)
                    {
                        return true;
                    }
                }
                
            }

            return false;
        }
        
        public int SumRemaining() => _board.Sum(r => r.Where(k => k >= 0).Sum());
    }
    
    [Fact]
    public void Puzzle1()
    {
        var inputs = "day4.txt".ReadInputLines();
        var values = new Queue<int>(inputs[0].Split(',').Select(int.Parse));
        var index = 2;
        var boards = new List<BingoBoard>();
        while (index < inputs.Length)
        {
            var rawBoard = inputs[index..(index + 5)];
            boards.Add(new BingoBoard(rawBoard));
            index += 6;
        }

        while (values.TryDequeue(out var next))
        {
            foreach (var board in boards)
            {
                if (board.MarkBoard(next))
                {
                    _testOutputHelper.WriteLine((board.SumRemaining() * next).ToString());
                    return;
                }
            }
        }
    }
    
    [Fact]
    public void Puzzle2()
    {
        var inputs = "day4.txt".ReadInputLines();
        var values = new Queue<int>(inputs[0].Split(',').Select(int.Parse));
        var index = 2;
        var boards = new List<BingoBoard>();
        while (index < inputs.Length)
        {
            var rawBoard = inputs[index..(index + 5)];
            boards.Add(new BingoBoard(rawBoard));
            index += 6;
        }

        BingoBoard lastBoard = null;

        while (values.TryDequeue(out var next) && boards.Count > 0)
        {
            foreach (var board in boards.ToArray())
            {
                if (board.MarkBoard(next))
                {
                    lastBoard = board;
                    boards.Remove(board);
                }
            }

            if (boards.Count == 0)
            {
                _testOutputHelper.WriteLine((lastBoard.SumRemaining() * next).ToString());
            }
        }
    }
}