using System.Reflection;

namespace Advent2021;

public static class HelperExtensions
{
    internal static int CharToInt(this char element) => int.Parse(element.ToString());
    
    internal static IEnumerable<(int Index, T Element)> Each<T>(this IEnumerable<T> elements)
    {
        var counter = 0;
        foreach (var e in elements)
        {
            yield return (counter, e);
            counter++;
        }
    }

    internal static string[] ReadInputLines(this string inputName) => 
        File.ReadAllLines(inputName.FindInput());

    internal static int[] ReadSingleIntCsv(this string inputName) => 
        File.ReadAllLines(inputName.FindInput()).First().Split(',')
            .Select(int.Parse).ToArray();

    internal static T[][] ParseGrid<T>(this string[] elements, Func<string, IEnumerable<T>> elementParser) =>
        elements.Select(f=>elementParser(f).ToArray()).ToArray();
    
    internal static string FindInput(this string inputName, string? path = null)
    {
        path ??= Assembly.GetExecutingAssembly().Location;
        var current = Directory.GetParent(path);
        var filename = $"{current?.FullName}/input/{inputName}";
        if (current != null)
        {
            if (File.Exists(filename))
            {
                return filename;
            }
            else
            {
                return FindInput(inputName, current.FullName);
            }
        }
        else
        {
            throw new FileNotFoundException();
        }
    }
}