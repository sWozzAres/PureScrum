using System.Net;
using Google.Api;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Scrum.Web.Blazor.Services;
using ScrumApp;
using ScrumApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CachedHandler>();
builder.Services.AddScoped<BrowserCachedHandler>();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<ITokenProvider, AppTokenProvider>();

builder.Services.AddHttpClient("CachedClient", client => client.BaseAddress = new Uri("https://localhost:7195"))
    .AddHttpMessageHandler(sp => sp.GetRequiredService<CachedHandler>());

builder.Services.AddHttpClient("BrowserCachedClient", client => client.BaseAddress = new Uri("https://localhost:7195"))
    .AddHttpMessageHandler(sp => sp.GetRequiredService<BrowserCachedHandler>());

var authorizedUrls = new[] { "https://localhost:7195" };
var scopes = new[] { "purescrum.client" };

//builder.Services
//    .AddGrpcClient<ScrumApi.ProductService.ProductServiceClient>(o =>
//    {
//        o.Address = new Uri("https://localhost:5001");
//    })
//    .AddCallCredentials(async (context, metadata, serviceProvider) =>
//    {
//        var provider = serviceProvider.GetRequiredService<ITokenProvider>();
//        var token = await provider.GetTokenAsync(context.CancellationToken);
//        metadata.Add("Authorization", $"Bearer {token}");
//    });

builder.Services.AddScoped(services =>
{
    var credentials = CallCredentials.FromInterceptor(async (context, metadata) =>
    {
        var provider = services.GetRequiredService<ITokenProvider>();
        var token = await provider.GetTokenAsync(context.CancellationToken);
        metadata.Add("Authorization", $"Bearer {token}");
    });

    var scrumApiUri = services.GetRequiredService<IConfiguration>()["ScrumApiUri"] ?? throw new InvalidOperationException("Invalid configuration.");
    var client = new ScrumApi.ProductService.ProductServiceClient(
        GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
            HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
        }));

    return client;
});

builder.Services.AddScoped(services =>
{
    var credentials = CallCredentials.FromInterceptor(async (context, metadata) =>
    {
        var provider = services.GetRequiredService<ITokenProvider>();
        var token = await provider.GetTokenAsync(context.CancellationToken);
        metadata.Add("Authorization", $"Bearer {token}");
    });
    var scrumApiUri = services.GetRequiredService<IConfiguration>()["ScrumApiUri"] ?? throw new InvalidOperationException("Invalid configuration.");
    return new ScrumApi.ProductBacklogItemService.ProductBacklogItemServiceClient(
        GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
            HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
        }));
});
builder.Services.AddScoped(services =>
{
    var credentials = CallCredentials.FromInterceptor(async (context, metadata) =>
    {
        var provider = services.GetRequiredService<ITokenProvider>();
        var token = await provider.GetTokenAsync(context.CancellationToken);
        metadata.Add("Authorization", $"Bearer {token}");
    });
    var scrumApiUri = services.GetRequiredService<IConfiguration>()["ScrumApiUri"] ?? throw new InvalidOperationException("Invalid configuration.");
    return new ScrumApi.SprintService.SprintServiceClient(
        GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
            HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
        }));
});

builder.Services.AddScoped(services =>
{
    var credentials = CallCredentials.FromInterceptor(async (context, metadata) =>
    {
        var provider = services.GetRequiredService<ITokenProvider>();
        var token = await provider.GetTokenAsync(context.CancellationToken);
        metadata.Add("Authorization", $"Bearer {token}");
    });
    var scrumApiUri = services.GetRequiredService<IConfiguration>()["ScrumApiUri"] ?? throw new InvalidOperationException("Invalid configuration.");
    return new ScrumApi.SprintBacklogItemService.SprintBacklogItemServiceClient(
            GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
            }));
});



builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri("https://localhost:7195"))
    .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
    .ConfigureHandler(authorizedUrls, scopes));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7195")});

builder.Services.AddSingleton<TabService>();
builder.Services.AddSingleton<CacheKeyService>();
builder.Services.AddSingleton<RememberService>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("oidc", options.ProviderOptions);
});

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();