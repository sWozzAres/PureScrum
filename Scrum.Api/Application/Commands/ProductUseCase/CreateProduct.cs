namespace Scrum.Api.Application.Commands.ProductUseCase;

public class CreateProduct(ScrumDbContext dbContext,
    ILogger<CreateProduct> logger) : UseCaseBase<Product>(dbContext)
{
    public Product Create(CreateProductRequest command)
    {
        logger.LogInformation("Creating product, request {@createproductrequest}.", command);

        var product = new Product(command.Name, command.Vision);
        DbContext.Products.Add(product);

        return product;
    }
}
