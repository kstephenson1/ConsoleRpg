using System.Globalization;

namespace ConsoleRpgEntities.Services.DataHelpers;

// The DataHelper.StringFuctions class contains string manipulation methods.
public static class StringHelper
{
    /// <summary>
    /// Takes the string and returns a variant in Titlecase
    /// </summary>
    /// <param name="input">The string to be converted to Titlecase</param>
    /// <returns>Uppercase <strong>String</strong> provided by the user.</returns>
    public static string ToTitleCase(string input) => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(input);
    [Obsolete]
    public static string ToItemIdFormat(string text) => text.Replace(" ", "_").ToLower();
    public static string ToItemNameFormat(string text) => ToTitleCase(text.Replace("_", " ").ToLower());
}
