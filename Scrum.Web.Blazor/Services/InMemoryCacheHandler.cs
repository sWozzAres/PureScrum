using System.Net;
using Microsoft.Extensions.Caching.Memory;

namespace Scrum.Web.Blazor.Services;

/// <summary>
/// Caches the request in memory.
/// </summary>
public class InMemoryCacheHandler(IMemoryCache cache, ILogger<InMemoryCacheHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //var queryString = HttpUtility.ParseQueryString(request.RequestUri!.Query);
        //var d = queryString["d"] 
        //    ?? throw new InvalidOperationException("You must supply a date parameter.");

        if (request.RequestUri is null)
            throw new InvalidOperationException("RequestUri is invalid.");

        var key = request.RequestUri.ToString();

        var cached = cache.Get<string>(key);
        if (cached is not null)
        {
            logger.LogInformation("Getting resource from cache.");
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(cached)
            };
        }
        logger.LogInformation("Resource is not cached.");

        var response = await base.SendAsync(request, cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        cache.Set(key, content, TimeSpan.FromDays(1));

        return response;
    }
}
