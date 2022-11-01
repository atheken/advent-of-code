using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day8
{
    private readonly ITestOutputHelper _output;

    public Day8(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    private class DisplayInstramentation
    {
        private static Regex LOCATOR = new("([a-g]+)");

        public DisplayInstramentation(string rawInput)
        {
            var inputs = rawInput.Split('|').ToArray();
            InputSignals = LOCATOR.Matches(inputs[0])
                .Select(k => new String(k.Groups[0].Value.OrderBy(f => f).ToArray())).ToArray();
            OutputSignals = LOCATOR.Matches(inputs[1])
                .Select(k => new String(k.Groups[0].Value.OrderBy(f => f).ToArray())).ToArray();
        }

        public string[] OutputSignals { get; set; }

        public string[] InputSignals { get; set; }

        /// <summary>
        /// If the right string fully intersects with the left, return true, else, false.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private bool Intersect(string left, string right)
        {
            foreach (var c in right)
            {
                if (!left.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetOutputValue()
        {
            var mapping = new Dictionary<int, string>()
            {
                [1] = InputSignals.First(k => k.Length == 2),
                [4] = InputSignals.First(k => k.Length == 4),
                [7] = InputSignals.First(k => k.Length == 3),
                [8] = InputSignals.First(k => k.Length == 7),
            };

            mapping[3] = InputSignals.First(k => k.Length == 5 && Intersect(k, mapping[7]));
            
            mapping[9] = InputSignals.First(k => k.Length == 6 && Intersect(k, mapping[3]));
            mapping[6] = InputSignals.First(k => k.Length == 6 && !Intersect(k, mapping[7]));
            
            mapping[5] = InputSignals.First(k => k.Length == 5 && Intersect(mapping[6], k));
            mapping[2] = InputSignals.First(k => k.Length == 5 && k != mapping[5] && k != mapping[3]);

            mapping[0] = InputSignals.First(k => k.Length == 6 && k != mapping[6] && k != mapping[9]);
            
            var inverted = mapping.ToDictionary(k => k.Value, v => v.Key);

            var retval = 0;
            //deduce from all inputs the possible outputs and then map each integer to an output.
            foreach (var x in OutputSignals)
            {
                retval *= 10;
                retval += inverted[x];
            }

            return retval;
        }
    }


    [Fact]
    public void Puzzle1()
    {
        var displayInfo = "day8.txt".ReadInputLines().Select(k => new DisplayInstramentation(k));
        var digits = displayInfo.SelectMany(k => k.OutputSignals)
            .Count(k => k.Length == 2 || k.Length == 3 || k.Length == 4 || k.Length == 7);
        _output.WriteLine(digits.ToString());
    }

    [Fact]
    public void Puzzle2()
    {
        var displayInfo = "day8.txt".ReadInputLines().Select(k => new DisplayInstramentation(k)).ToArray();
        var digits = displayInfo.Sum(k => k.GetOutputValue());

        _output.WriteLine(digits.ToString());
    }
}