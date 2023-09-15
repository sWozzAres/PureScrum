using System.Reflection;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Scrum.Api.Domain.Infrastructure;
using Scrum.Api.Extensions;
using Scrum.Web.Api.Configuration;
using Scrum.Web.Api.Extensions;
using Scrum.Web.Api.Identity;
using Scrum.Web.Api.Infrastructure;
using Scrum.Web.Api.Infrastructure.Seed;
using Scrum.Web.Api.Server;
using Scrum.Web.Api.Services;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Scrum.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string CorsPolicy = "CorsPolicy";
            const string AppName = "Scrum.Api";

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            Log.Logger = new LoggerConfiguration()
                    //.MinimumLevel.Verbose()
                    .Enrich.WithProperty("ApplicationContext", AppName)
                    .Enrich.FromLogContext()
                    //.WriteTo.Console()
                    //.WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                    //.WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
                    .ReadFrom.Configuration(builder.Configuration)
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                    .CreateLogger();


            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    options.Listen(IPAddress.Any, 5001, listenOptions =>
            //    {
            //        listenOptions.Protocols = HttpProtocols.Http2;
            //        //listenOptions.UseHttps("<path to .pfx file>",
            //        //    "<certificate password>");
            //    });
            //});

            builder.Services.AddAuthentication();
            //.AddBearerToken(IdentityConstants.BearerScheme);
            builder.Services.AddAuthorizationBuilder();
            builder.Services.AddDbContext<SecurityDbContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("SecurityDbConnection")));

            //builder.Services.AddIdentityCore<ScrumUser>()
            //    .AddEntityFrameworkStores<SecurityDbContext>()
            //    .AddApiEndpoints();
            //https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-dotnet-8-preview-7/comment-page-2/#comments
            builder.Services.AddIdentityApiEndpoints<ScrumUser>()
                .AddEntityFrameworkStores<SecurityDbContext>();


            builder.Services.AddMemoryCache();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicy,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:7289", "https://localhost:44424", "http://localhost:4200");
                                      policy.AllowAnyHeader();
                                      policy.AllowAnyMethod();
                                      policy.WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
                                  });
            });

            // Add services to the container.
            builder.Services.AddScrumApplication(
                builder.Configuration.GetConnectionString("ScrumDbConnection")
                    ?? throw new InvalidOperationException("Failed to load connection string ScrumDbConnection."));

            builder.Services.AddApplicationSecurity();

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(Scrum.Api.Controllers.ProductController).GetTypeInfo().Assembly);

            builder.Services.AddGrpc().AddJsonTranscoding();
            //builder.Services.AddResponseCompression(opts =>
            //{
            //    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            //        new[] { "application/octet-stream" });
            //});

            var app = builder.Build();

            app.MapIdentityApi<ScrumUser>();

            //TODO
            app.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}").RequireAuthorization();

            app.MigrateDbContext<ScrumDbContext>((context, services) =>
            {
                var env = services.GetRequiredService<IWebHostEnvironment>();
                var logger = services.GetRequiredService<ILogger<ScrumDbContext>>();

                new ScrumDbContextSeeder(context, services)
                    .SeedAsync(env)
                    .Wait();
            });

            // Configure the HTTP request pipeline.

            //app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });


            //_ = app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGrpcService<ProductService>();
            //});

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseCors(CorsPolicy);
            app.UseAuthorization();



            app.UseGrpcWeb();



            app.MapGrpcService<ProductService>().EnableGrpcWeb();
            app.MapGrpcService<ProductBacklogItemService>().EnableGrpcWeb();
            app.MapGrpcService<SprintService>().EnableGrpcWeb();
            app.MapGrpcService<SprintBacklogItemService>().EnableGrpcWeb();

            app.MapControllers();

            app.Run();
        }
    }
}
