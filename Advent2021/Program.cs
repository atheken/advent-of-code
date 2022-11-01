// See https://aka.ms/new-console-template for more information

using System.Reflection;

public class Program
{
    private static string FindInput(string inputName, string? path = null)
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
    
    public static async Task Main()
    {
        var input = File.ReadLines(FindInput("puzzle1.txt"));
        int? previous = null;
        var increasingCount = 0;
        
        foreach (var i in input.Select(k => int.Parse(k)))
        {
            if (previous.HasValue && i > previous)
            {
                increasingCount++;
            }
            previous = i;
        }
        Console.WriteLine(increasingCount);
    }
}