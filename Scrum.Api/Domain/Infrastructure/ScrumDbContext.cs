using Scrum.Api.Domain.Configuration;

namespace Scrum.Api.Domain.Infrastructure;

public class ScrumDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sprint> Sprints => Set<Sprint>();
    //public DbSet<SprintEstimation> SprintEstimations => Set<SprintEstimation>();
    public DbSet<ProductBacklogItem> ProductBacklogItems => Set<ProductBacklogItem>();
    public DbSet<ProductBacklogItemDependency> ProductBacklogItemDependencies => Set<ProductBacklogItemDependency>();
    public DbSet<SprintBacklogItem> SprintBacklogItems => Set<SprintBacklogItem>();
    public DbSet<Impediment> Impediments => Set<Impediment>();
    public DbSet<DefinitionOfDone> DefinitionOfDones => Set<DefinitionOfDone>();
    public ScrumDbContext(DbContextOptions<ScrumDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductBacklogItemEntityTypeConfiguration).Assembly);
    }
}
