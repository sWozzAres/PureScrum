using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Scrum.Web.Blazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Scrum.Web.Blazor.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, string webApiUrl)
    {
        var authorizedUrls = new[] { webApiUrl };
        var scopes = new[] { "purescrum.client" };

        // named HTTP client
        const string clientName = "WebAPI";

        services.AddHttpClient(clientName, client => client.BaseAddress = new Uri(webApiUrl))
            .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(authorizedUrls, scopes));

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient(clientName));

        // caching clients
        services.AddHttpClient("CachedClient", client => client.BaseAddress = new Uri(webApiUrl))
            .AddHttpMessageHandler(sp => sp.GetRequiredService<InMemoryCacheHandler>())
            .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(authorizedUrls, scopes));

        services.AddHttpClient("BrowserCachedClient", client => client.BaseAddress = new Uri(webApiUrl))
            .AddHttpMessageHandler(sp => sp.GetRequiredService<BrowserCachedHandler>())
            .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(authorizedUrls, scopes));

        return services;
    }
    public static IServiceCollection AddGrpcServices(this IServiceCollection services,
        string scrumApiUri)
    {
        services.AddScoped<ITokenProvider, AppTokenProvider>();

        services.AddScoped(services =>
        {
            var credentials = CallCredentials.FromInterceptor(async (context, metadata) =>
            {
                var token = await services
                    .GetRequiredService<ITokenProvider>()
                    .GetTokenAsync(context.CancellationToken);
                metadata.Add("Authorization", $"Bearer {token}");
            });

            var client = new ScrumApi.ProductService.ProductServiceClient(
                GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
                {
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                    HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
                }));

            return client;
        });
        services.AddScoped(services =>
        {
            var credentials = CallCredentials.FromInterceptor(async (context, metadata) =>
            {
                var token = await services
                    .GetRequiredService<ITokenProvider>()
                    .GetTokenAsync(context.CancellationToken);
                metadata.Add("Authorization", $"Bearer {token}");
            });
            return new ScrumApi.ProductBacklogItemService.ProductBacklogItemServiceClient(
                GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
                {
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                    HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
                }));
        });
        services.AddScoped(services =>
        {
            var credentials = CallCredentials.FromInterceptor(async (context, metadata) =>
            {
                var token = await services
                     .GetRequiredService<ITokenProvider>()
                     .GetTokenAsync(context.CancellationToken);
                metadata.Add("Authorization", $"Bearer {token}");
            });
            return new ScrumApi.SprintService.SprintServiceClient(
                GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
                {
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                    HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
                }));
        });
        services.AddScoped(services =>
        {
            var credentials = CallCredentials.FromInterceptor(async (context, metadata) =>
            {
                var token = await services
                    .GetRequiredService<ITokenProvider>()
                    .GetTokenAsync(context.CancellationToken);
                metadata.Add("Authorization", $"Bearer {token}");
            });
            return new ScrumApi.SprintBacklogItemService.SprintBacklogItemServiceClient(
                    GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
                    {
                        Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                        HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
                    }));
        });

        return services;
    }
}
