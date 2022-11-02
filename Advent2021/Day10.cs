using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day10
{
    private readonly ITestOutputHelper _output;

    public Day10(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    private class Chunk
    {
        private static readonly Regex Reducer = new("(\\{})|(\\[])|(\\<>)|(\\(\\))");

        public static string Reduce(string subject)
        {
            int previousLength;

            do
            {
                previousLength = subject.Length;
                subject = Reducer.Replace(subject, "");
            } while (previousLength != subject.Length);

            return subject;
        }
    }

    [Fact]
    public void Puzzle1()
    {

        var score = "day10.txt".ReadInputLines().Select(k => Chunk.Reduce(k))
            .Select(k=> k.TrimStart('[','{','<','('))
            .Where(k => k.Length > 0)
            .Sum(f => f[0] switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
            });

        _output.WriteObject(score);
    }

    [Fact]
    public void Puzzle2()
    {
        var scoreMap = new Dictionary<char, int>
        {
            ['('] = 1,
            ['['] = 2,
            ['{'] = 3,
            ['<'] = 4,
        };
        
        var score = "day10.txt".ReadInputLines().Select(k => Chunk.Reduce(k))
            //if the remainder is only opening values,
            .Where(k => k.TrimStart('[', '{', '<', '(').Length == 0)
            .Select( f=> f.Reverse())
            .Select( f=> f.Aggregate(0L, (seed, current) => seed * 5 + scoreMap[current]))
            .OrderBy(k=>k).ToArray();

        _output.WriteObject(score[(int) Math.Floor(score.Length / 2f)]);
    }
}