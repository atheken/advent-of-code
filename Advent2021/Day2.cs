using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Advent2021;

public class Day2
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day2(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Puzzle1()
    {
        var regex = new Regex("^(?<command>[^\\d]+) (?<magnitude>[\\d]+)$", RegexOptions.ExplicitCapture);
        
        var commands = "day2.txt".ReadInputLines()
            .Select(k => regex.Match(k)).Select(k => (
                command: k.Groups["command"].Value,
                value: int.Parse(k.Groups["magnitude"].Value
                )));
        
        var horizontalMovement = 0;
        var verticalMovement = 0;

        foreach (var (command, value) in commands)
        {
            switch (command)
            {
                case "up":
                    verticalMovement -= value;
                    break;
                case "down":
                    verticalMovement += value;
                    break;
                case "forward":
                    horizontalMovement += value;
                    break;
            }

            ;
        }

        _testOutputHelper.WriteLine((horizontalMovement * verticalMovement).ToString());
    }
    
    [Fact]
    public void Puzzle2()
    {
        var regex = new Regex("^(?<command>[^\\d]+) (?<magnitude>[\\d]+)$", RegexOptions.ExplicitCapture);
        
        var commands = "day2.txt".ReadInputLines()
            .Select(k => regex.Match(k)).Select(k => (
                command: k.Groups["command"].Value,
                value: int.Parse(k.Groups["magnitude"].Value
                )));
        
        var horizontalMovement = 0;
        var depth = 0;
        var aim = 0;

        foreach (var (command, value) in commands)
        {
            switch (command)
            {
                case "up":
                    aim -= value;
                    break;
                case "down":
                    aim += value;
                    break;
                case "forward":
                    horizontalMovement += value;
                    depth += value * aim;
                    break;
            }

            ;
        }

        _testOutputHelper.WriteLine((horizontalMovement * depth).ToString());
    }
}