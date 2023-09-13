using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Scrum.Web.Blazor.Services;
using ScrumApp;
using ScrumApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CachedHandler>();
builder.Services.AddScoped<BrowserCachedHandler>();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("CachedClient", client => client.BaseAddress = new Uri("https://localhost:7195"))
    .AddHttpMessageHandler(sp => sp.GetRequiredService<CachedHandler>());

builder.Services.AddHttpClient("BrowserCachedClient", client => client.BaseAddress = new Uri("https://localhost:7195"))
    .AddHttpMessageHandler(sp => sp.GetRequiredService<BrowserCachedHandler>());


builder.Services.AddSingleton(services =>
{
    var scrumApiUri = services.GetRequiredService<IConfiguration>()["ScrumApiUri"] ?? throw new InvalidOperationException("Invalid configuration.");
    return new ScrumApi.ProductService.ProductServiceClient(
        GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
        {
            HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
        }));
});
builder.Services.AddSingleton(services =>
{
    var scrumApiUri = services.GetRequiredService<IConfiguration>()["ScrumApiUri"] ?? throw new InvalidOperationException("Invalid configuration.");
    return new ScrumApi.ProductBacklogItemService.ProductBacklogItemServiceClient(
        GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
        {
            HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
        }));
});
builder.Services.AddSingleton(services =>
{
    var scrumApiUri = services.GetRequiredService<IConfiguration>()["ScrumApiUri"] ?? throw new InvalidOperationException("Invalid configuration.");
    return new ScrumApi.SprintService.SprintServiceClient(
        GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
        {
            HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
        }));
});

builder.Services.AddSingleton(services =>
{
    var scrumApiUri = services.GetRequiredService<IConfiguration>()["ScrumApiUri"] ?? throw new InvalidOperationException("Invalid configuration.");
    return new ScrumApi.SprintBacklogItemService.SprintBacklogItemServiceClient(
            GrpcChannel.ForAddress(scrumApiUri, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
            }));
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7195") });
builder.Services.AddSingleton<TabService>();
builder.Services.AddSingleton<CacheKeyService>();
builder.Services.AddSingleton<RememberService>();

await builder.Build().RunAsync();