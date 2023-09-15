using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Scrum.Web.Blazor.Services;

public interface ITokenProvider
{
    Task<string> GetTokenAsync(CancellationToken cancellationToken);
}

public class AppTokenProvider(IAccessTokenProvider tokenProvider) : ITokenProvider
{
    string? _token;
    readonly IAccessTokenProvider _tokenProvider = tokenProvider;

    public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        if (_token == null)
        {
            var accessTokenResult = await _tokenProvider.RequestAccessToken();
            if (!accessTokenResult.TryGetToken(out var token))
                throw new InvalidOperationException("Failed to get token from token provider.");

            _token = token.Value;
        }

        return _token;
    }

    public void Reset() => _token = null;
}