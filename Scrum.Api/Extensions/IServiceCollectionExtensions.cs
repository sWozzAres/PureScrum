using System.Reflection;
using Scrum.Api.Application.Commands.ProductBacklogItemUseCase;
using Scrum.Api.Application.Commands.ProductUseCase;
using Scrum.Api.Application.Commands.SprintBacklogItemUseCase;
using Scrum.Api.Application.Commands.SprintUseCase;
using Scrum.Api.Application.Queries;

namespace Scrum.Api.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddScrumApplication(this IServiceCollection services, string connectionString)
    {
        // queries
        services.AddTransient<ProductQueries>();
        services.AddTransient<SprintQueries>();
        services.AddTransient<ProductBacklogItemQueries>();
        services.AddTransient<SprintBacklogItemQueries>();
        services.AddTransient<ImpedimentQueries>();
        services.AddTransient<DefinitionOfDoneQueries>();

        // product
        services.AddTransient<CreateProduct>();
        services.AddTransient<UpdateProduct>();
        services.AddTransient<DeleteProduct>();

        // sprint
        services.AddTransient<CreateSprint>();
        services.AddTransient<UpdateSprint>();
        services.AddTransient<DeleteSprint>();

        // product backlog item
        services.AddTransient<CreateProductBacklogItem>();
        services.AddTransient<UpdateProductBacklogItem>();
        services.AddTransient<AddProductBacklogItemDependencies>();
        services.AddTransient<RemoveProductBacklogItemDependency>();
        services.AddTransient<DeleteProductBacklogItem>();

        // sprint backlog item
        services.AddTransient<CreateSprintBacklogItem>();
        services.AddTransient<UpdateSprintBacklogItem>();
        services.AddTransient<DeleteSprintBacklogItem>();
        services.AddTransient<MoveToPbi>();

        // dbcontext
        services.AddDbContext<ScrumDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();

            options.UseSqlServer(connectionString, sqlOptions =>
            {
                //sqlOptions.UseNetTopologySuite();
                sqlOptions.UseDateOnlyTimeOnly();
                sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                sqlOptions.EnableRetryOnFailure();
            });
        });

        return services;
    }
}
