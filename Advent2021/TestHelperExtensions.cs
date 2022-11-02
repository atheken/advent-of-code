using Xunit.Abstractions;
using System.Text.Json;

namespace Advent2021;

public static class TestHelperExtensions
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        WriteIndented = true
    };
    
    /// <summary>
    /// Writes the object as json to the output.
    /// </summary>
    /// <param name="output"></param>
    /// <param name="element"></param>
    public static void WriteObject(this ITestOutputHelper output, object element)
    {
        if(element is string e)
        {
            output.WriteLine(e);
        }
        else
        {
            output.WriteLine(JsonSerializer.Serialize(element, Options));
        }
    }
}