namespace Scrum.Api.Helpers;

public static class ApiHelpers
{
    /// <summary>
    /// Turns a string in the form "key=value, key=value, ..." into a Dictionary
    /// </summary>
    /// <param name="input"></param>
    /// <returns>null if the input is invalid, an empty Dictionary if the input is null or the Dictionary</returns>
    public static Dictionary<string, string>? ParseStringToDictionary(string? input)
    {
        var dictionary = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(input))
            return dictionary;

        // Split the input string by comma to get individual key-value pairs
        string[] keyValuePairs = input.Split(',');

        foreach (string pair in keyValuePairs)
        {
            // Split each key-value pair by '=' to get the key and value
            string[] parts = pair.Split('=');

            if (parts.Length == 2)
            {
                string key = parts[0];
                string value = parts[1];

                dictionary[key] = value;
            }
            else
            {
                return null;
            }
        }

        return dictionary;
    }
}
