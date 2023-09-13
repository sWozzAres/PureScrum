namespace Scrum.Api.Application.Commands.ProductBacklogItemUseCase;

public class RemoveProductBacklogItemDependency(ScrumDbContext dbContext)
{
    public void Remove(Guid productBacklogItemId, RemoveDependencyRequest command)
    {
        var d = new ProductBacklogItemDependency(productBacklogItemId, command.ProductBacklogItemId!.Value);
        dbContext.Set<ProductBacklogItemDependency>("ProductBacklogItemDependencies").Remove(d);
    }
}
