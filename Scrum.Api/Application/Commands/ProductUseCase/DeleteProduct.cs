namespace Scrum.Api.Application.Commands.ProductUseCase;

public class DeleteProduct(ScrumDbContext dbContext,
    ILogger<DeleteProduct> logger) : UseCaseBase<Product>(dbContext)
{
    public async Task<bool> DeleteAsync(Guid productId, CancellationToken ct)
    {
        logger.LogInformation("Deleting product {productId}.", productId);

        var product = await FindAsync(productId, ct);
        if (product is null)
            return false;

        DbContext.Products.Remove(product);
        return true;

        //var loaded = await InitializeAndTryLoadAsync(productId, ct);
        //if (loaded)
        //    DbContext.Products.Remove(Entity);
        //return loaded;
    }
}
