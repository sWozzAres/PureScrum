using System.Text.RegularExpressions;

namespace ScrumApp.Extensions;

public static partial class ClrExtensions
{
    /// <summary>
    /// Converts a bool to an HTML string.
    /// </summary>
    public static string ToHtml(this bool b) => b ? "true" : "false";

    /// <summary>
    /// Converts a Camel Case named enum to a string, inserting spaces
    /// between capital letters.
    /// </summary>
    public static string ToDisplayString(this Enum @enum)
        => MyRegex().Replace(@enum.ToString(), "$1 $2");

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}
