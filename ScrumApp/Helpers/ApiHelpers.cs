using System.Text.Json;
using ScrumApp.Services;

namespace ScrumApp.Helpers;

public static class ApiHelpers
{
    public static async Task<PostResponse> DeleteIdsAsync(Dictionary<Guid, bool> checkedIds, HttpClient http, string url)
        => await DeleteIdsAsync(checkedIds.Where(c => c.Value).Select(c => c.Key.ToString()), http, url);

    public static async Task<PostResponse> DeleteIdsAsync(Dictionary<string, bool> checkedIds, HttpClient http, string url)
        => await DeleteIdsAsync(checkedIds.Where(c => c.Value).Select(c => c.Key), http, url);

    public static async Task<PostResponse> DeleteIdsAsync(IEnumerable<string> toDelete,
        HttpClient http, string url)
    {
        if (!toDelete.Any())
            throw new InvalidOperationException("Nothing to delete.");

        var request = new HttpRequestMessage(HttpMethod.Delete, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(toDelete.ToList()),
            System.Text.Encoding.UTF8, "application/json")
        };

        return PostResponse.Create(await http.SendAsync(request));
    }
}
