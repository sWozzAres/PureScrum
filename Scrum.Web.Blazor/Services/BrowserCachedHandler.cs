using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Scrum.Web.Blazor.Services;

public sealed class BrowserCachedHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestMode(BrowserRequestMode.Cors);
        request.SetBrowserRequestCache(BrowserRequestCache.ForceCache);

        return base.SendAsync(request, cancellationToken);
    }
}