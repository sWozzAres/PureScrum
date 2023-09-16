using System.Diagnostics.CodeAnalysis;

namespace Scrum.Web.Blazor.Services;

public class CacheKeyService
{
    readonly Dictionary<string, string> _cacheKeys = new();
    /// <summary>
    /// Adds or updates the specified key in the cache with a new randomly 
    /// generated value.
    /// </summary>
    /// <returns>The randomly generated value.</returns>
    public string AddOrUpdate(string key)
    {
        var value = ScrumApp.Helpers.HtmlHelpers.RandomCacheKey;
        if (!_cacheKeys.TryAdd(key, value))
        {
            _cacheKeys[key] = value;
        }

        return value;
    }
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
        => _cacheKeys.TryGetValue(key, out value);

}
