using Scrum.Api.Exceptions;

namespace Scrum.Api.Application.Commands.ProductUseCase;

public class UpdateProduct(
    ScrumDbContext dbContext,
    ILogger<UpdateProduct> logger) : UseCaseBase<Product>(dbContext)
{
    public async Task<bool> UpdateAsync(Guid productId, UpdateProductRequest request, CancellationToken ct)
    {
        logger.LogInformation("Updating product {productid}, request {@updateproductrequest}.", productId, request);

        var product = await FindAsync(productId, ct)
            ?? throw new ScrumDomainException("Failed to load Product.");

        //await InitializeAndLoadAsync(productId, ct);

        product.Name = request.Name;
        product.Vision = request.Vision;

        return DbContext.Entry(product).State == EntityState.Modified;
    }
}
