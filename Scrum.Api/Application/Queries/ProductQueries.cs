namespace Scrum.Api.Application.Queries;

public class ProductQueries(ScrumDbContext dbContext)
{
    public async Task<ProductFullDto?> GetProductAsync(Guid productId, CancellationToken ct)
    {
        var query = from p in dbContext.Products
                    where p.Id == productId
                    select new
                    {
                        p.Id,
                        p.Name,
                        p.Vision
                    };

        var entity = await query
            .SingleOrDefaultAsync(ct);

        return entity is null
            ? null
            : new(entity.Id, entity.Name, entity.Vision);
    }
    public async Task<List<ProductListDto>> GetProductsAsync(CancellationToken ct)
    {
        var query = from p in dbContext.Products
                    select new
                    {
                        p.Id,
                        p.Name,
                        p.Vision
                    };

        var entities = await query
            .ToListAsync(ct);

        return entities.Select(e => new ProductListDto(e.Id, e.Name, e.Vision)).ToList();
    }
    public Task<List<ProductListShortDto>> GetProductsShortAsync(CancellationToken ct)
    {
        var query = from p in dbContext.Products
                    select new ProductListShortDto(
                        p.Id,
                        p.Name
                    );

        return query.ToListAsync(ct);
    }
}
