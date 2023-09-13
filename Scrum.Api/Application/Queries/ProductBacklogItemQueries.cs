using Scrum.Api.Helpers;

namespace Scrum.Api.Application.Queries;

public class ProductBacklogItemQueries(ScrumDbContext dbContext, ILogger<ProductBacklogItemQueries> logger)
{
    public async Task<ProductBacklogItemFullDto?> GetProductBacklogItemAsync(Guid productBacklogItemId, CancellationToken ct)
    {
        var query = from p in dbContext.ProductBacklogItems
                    join __s in dbContext.Sprints on p.SprintId equals __s.Id into _s
                    from s in _s.DefaultIfEmpty()

                    join pr in dbContext.Products on p.ProductId equals pr.Id


                    where p.Id == productBacklogItemId
                    select new
                    {
                        p.Id,
                        p.ProductId,
                        ProductName = pr.Name,
                        p.Status,
                        p.Name,
                        p.Description,
                        p.EstimationPoints,
                        p.Notes,
                        p.DeliveryDate,
                        p.IsFixedDeliveryDate,
                        p.Value,
                        p.Roi,
                        p.SprintId,
                        SprintName = s == null ? null : s.Name,
                        Children = p.Children.Select(d => new { d.Id, d.Name, d.Status }),
                        SprintBacklogItems = p.SprintBacklogItems.Select(s => new { s.Id, s.Name, s.Description, s.EstimationPoints, s.Status })
                    };

        var entity = await query
            .SingleOrDefaultAsync(ct);

        return entity is null
            ? null
            : new(entity.Id, entity.ProductId, entity.ProductName, (ProductBacklogItemStatusDto)entity.Status, entity.Name,
            entity.Description, entity.EstimationPoints, entity.Notes, entity.DeliveryDate, entity.IsFixedDeliveryDate,
            entity.Value, entity.Roi, entity.SprintId, entity.SprintName,
            entity.Children.Select(d => new ProductBacklogItemDependentDto(d.Id, d.Name, (ProductBacklogItemStatusDto)d.Status)).ToList(),
            entity.SprintBacklogItems.Select(s => new SprintBacklogItemShortDto(s.Id, s.Name, s.Description, s.EstimationPoints, (SprintBacklogItemStatusDto)s.Status)).ToList());
    }
    public async Task<List<ProductBacklogItemListDto>> GetProductBacklogItemsAsync(
        Guid? sprintId,
        Guid? productId,
        string? nameFilter,
        bool? open,
        CancellationToken ct)
    {
        var pbis = dbContext.ProductBacklogItems
            .Include(x => x.Children)
            .Include(x => x.Product)
            .Include(x => x.Sprint)
            .Include(x => x.SprintBacklogItems)
            .AsNoTracking()
            .AsQueryable();

        if (sprintId.HasValue)
        {
            pbis = pbis.Where(p => p.SprintId == sprintId.Value);
        }
        if (productId.HasValue)
        {
            pbis = pbis.Where(p => p.ProductId == productId.Value);
        }
        if (open is not null)
        {
            pbis = pbis.Where(p => p.Status == PbiStatus.None || p.Status == PbiStatus.Ready);
        }
        if (nameFilter is not null)
        {
            pbis = pbis.Where(s => s.Name.StartsWith(nameFilter));
        }

        var list = await pbis.ToListAsync(ct);

        return list.Select(e =>
            new ProductBacklogItemListDto(e.Id, e.Name, e.Description, (ProductBacklogItemStatusDto)e.Status,
                e.Product.Name,
                e.DeliveryDate,
                e.Sprint == null ? null : e.Sprint.Id,
                e.Sprint == null ? null : e.Sprint.Name,
                e.Value, e.Roi, e.EstimationPoints,
                e.Children
                    .Select(d => new ProductBacklogItemDependentDto(d.Id, d.Name, (ProductBacklogItemStatusDto)e.Status)).ToList(),
                e.SprintBacklogItems
                    .Select(s => new SprintBacklogItemShortDto(s.Id, s.Name, s.Description, s.EstimationPoints, (SprintBacklogItemStatusDto)s.Status)).ToList(),
                false
            )).ToList();

        //var query = from p in dbContext.ProductBacklogItems.Include(p => p.Children)
        //            join product in dbContext.Products on p.ProductId equals product.Id

        //            join __inc in dbContext.Sprints on p.SprintId equals __inc.Id into _inc
        //            from inc in _inc.DefaultIfEmpty()

        //            select new
        //            {
        //                p.Id,
        //                p.Name,
        //                p.Status,
        //                p.Description,
        //                ProductId = product.Id,
        //                ProductName = product.Name,
        //                GoalId = inc == null ? (Guid?)null : inc.Id,
        //                GoalName = inc == null ? null : inc.Name,
        //                p.Value,
        //                p.Roi,
        //                p.DeliveryDate,
        //                p.EstimationPoints,
        //                Children = p.Children.Select(d => new { d.Id, d.Name, d.Status }),
        //                SprintBacklogItems = p.SprintBacklogItems.Select(s => new { s.Id, s.Name, s.Description, s.EstimationPoints, s.Status })
        //            };

        //if (sprintIdFilter is not null)
        //{
        //    query = from q in query
        //            where q.GoalId == sprintIdFilter
        //            select q;
        //}
        //if (productIdFilter is not null)
        //{
        //    query = from q in query
        //            where q.ProductId == productIdFilter
        //            select q;
        //}

        //var entities = await query
        //    .ToListAsync(ct);

        //logger.LogInformation("Change tracker entries after ToList: {count}.", dbContext.ChangeTracker.Entries().Count());

        //return entities.Select(e =>
        //    new ProductBacklogItemListDto(e.Id, e.Name, e.Description, (ProductBacklogItemStatusDto)e.Status,
        //        e.ProductName, e.DeliveryDate, e.GoalId, e.GoalName, e.Value, e.Roi, e.EstimationPoints,
        //        e.Children.Select(d => new ProductBacklogItemDependentDto(d.Id, d.Name, (ProductBacklogItemStatusDto)e.Status)).ToList(),
        //        e.SprintBacklogItems.Select(s => new SprintBacklogItemShortDto(s.Id, s.Name, s.Description, s.EstimationPoints, (SprintBacklogItemStatusDto)s.Status)).ToList())).ToList();
    }

    public async Task<List<ProductBacklogItemListDto>> GetProductBacklogItemsAllAsync(
        Guid? sprintId,
        Guid? productId,
        bool? open,
        CancellationToken ct)
    {
        var pbis = dbContext.ProductBacklogItems
            .Include(x => x.Children)
            .Include(x => x.Product)
            .Include(x => x.Sprint)
            .Include(x => x.SprintBacklogItems)
            .AsNoTracking()
            .AsQueryable();

        if (sprintId.HasValue)
        {
            pbis = pbis.Where(p => p.SprintId == sprintId.Value);
        }
        if (productId.HasValue)
        {
            pbis = pbis.Where(p => p.ProductId == productId.Value);
        }
        if (open is not null)
        {
            pbis = pbis.Where(p => p.Status == PbiStatus.None || p.Status == PbiStatus.Ready);
        }

        var list = await pbis.ToListAsync(ct);

        var missing = await EntityHelpers.GetAll(list, (missing, cancellationToken) =>
        {
            var missingPbis = dbContext.ProductBacklogItems
                .Include(x => x.Children)
                .Include(x => x.Product)
                .Include(x => x.Sprint)
                .Include(x => x.SprintBacklogItems)
                .AsNoTracking()
                .Where(x => missing.Contains(x.Id));

            return missingPbis.ToListAsync(cancellationToken);
        }
        , ct);

        return list.Select(e =>
            new ProductBacklogItemListDto(e.Id, e.Name, e.Description, (ProductBacklogItemStatusDto)e.Status,
                e.Product.Name,
                e.DeliveryDate,
                e.Sprint == null ? null : e.Sprint.Id,
                e.Sprint == null ? null : e.Sprint.Name,
                e.Value, e.Roi, e.EstimationPoints,
                e.Children
                    .Select(d => new ProductBacklogItemDependentDto(d.Id, d.Name, (ProductBacklogItemStatusDto)e.Status)).ToList(),
                e.SprintBacklogItems
                    .Select(s => new SprintBacklogItemShortDto(s.Id, s.Name, s.Description, s.EstimationPoints, (SprintBacklogItemStatusDto)s.Status)).ToList(),
                missing.Exists(m => m == e.Id)
            )).ToList();
    }
}
