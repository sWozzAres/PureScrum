using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Scrum.Web.Blazor.Services;

/// <summary>
/// Forces the browser to cache the request.
/// </summary>
public sealed class BrowserCachedHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestMode(BrowserRequestMode.Cors);
        request.SetBrowserRequestCache(BrowserRequestCache.ForceCache);

        return base.SendAsync(request, cancellationToken);
    }
}