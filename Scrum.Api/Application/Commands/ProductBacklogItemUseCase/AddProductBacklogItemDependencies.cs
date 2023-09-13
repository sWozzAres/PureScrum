namespace Scrum.Api.Application.Commands.ProductBacklogItemUseCase;

public class AddProductBacklogItemDependencies(ScrumDbContext dbContext)
{
    public void Add(Guid productBacklogItemId, List<Guid> dependencies)
    {
        foreach (var dependency in dependencies)
        {
            dbContext.Set<ProductBacklogItemDependency>("ProductBacklogItemDependencies").Add(
                new(productBacklogItemId, dependency));
        }
    }
}
