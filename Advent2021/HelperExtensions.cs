using System.Reflection;

namespace Advent2021;

public static class HelperExtensions
{
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