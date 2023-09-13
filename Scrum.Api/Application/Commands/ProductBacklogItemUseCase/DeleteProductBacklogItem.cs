namespace Scrum.Api.Application.Commands.ProductBacklogItemUseCase;

public class DeleteProductBacklogItem(ScrumDbContext dbContext)// : UseCaseBase<ProductBacklogItem>(dbContext)
{
    public async Task<bool> DeleteAsync(Guid productbacklogitemId, CancellationToken ct)
    {
        var pbi = await dbContext.ProductBacklogItems
                .Include(p => p.Children)
                .SingleOrDefaultAsync(p => p.Id == productbacklogitemId, ct);

        if (pbi is null)
            return false;

        // remove dependencies
        var children = pbi.Children.ToList();
        foreach (var d in children)
        {
            pbi.Children.Remove(d);
        }

        dbContext.ProductBacklogItems.Remove(pbi);
        return true;
    }
}
