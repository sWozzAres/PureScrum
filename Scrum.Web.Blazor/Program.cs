using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Scrum.Web.Blazor.Extensions;
using Scrum.Web.Blazor.Services;
using ScrumApp;
using ScrumApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<BrowserCachedHandler>(); 
builder.Services.AddScoped<InMemoryCacheHandler>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<TabService>();
builder.Services.AddSingleton<CacheKeyService>();
builder.Services.AddSingleton<RememberService>();

builder.Services.AddGrpcServices(builder.Configuration["ApiUrl_Grpc"] ?? throw new InvalidOperationException("No 'ApiUrl_Grpc' defined in configuration."));
builder.Services.AddWebApi(builder.Configuration["ApiUrl_Web"] ?? throw new InvalidOperationException("No 'ApiUrl_Web' defined in configuration."));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("oidc", options.ProviderOptions);
});

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();