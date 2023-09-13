using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Scrum.Server;
using Scrum.Server.Identity;
using Scrum.Server.Infrastructure;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
        //.MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", "Scrum.Server")
        .Enrich.FromLogContext()
        //.WriteTo.Console()
        //.WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
        //.WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
        .CreateLogger();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents()
    .AddWebAssemblyComponents();

builder.Services.AddAuthorization();
builder.Services.AddDbContext<SecurityDbContext>(c =>
    c.UseSqlServer(builder.Configuration.GetConnectionString("SecurityDbConnection")));
//https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-dotnet-8-preview-7/comment-page-2/#comments
builder.Services.AddIdentityApiEndpoints<ScrumUser>()
    .AddEntityFrameworkStores<SecurityDbContext>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7010") });

var app = builder.Build();

app.MapGroup("/identity").MapIdentityApi<ScrumUser>();
app.MapGet("/requires-auth", (ClaimsPrincipal user)
    => $"Hello, {user.Identity?.Name}!").RequireAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddServerRenderMode()
    .AddWebAssemblyRenderMode();

app.Run();
