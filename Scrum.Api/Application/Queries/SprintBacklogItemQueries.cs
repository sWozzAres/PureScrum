namespace Scrum.Api.Application.Queries;

public class SprintBacklogItemQueries(ScrumDbContext dbContext)
{
    public async Task<SprintBacklogItemFullDto?> GetSprintBacklogItemAsync(Guid sprintBacklogItemId, CancellationToken ct)
    {
        var query = from p in dbContext.SprintBacklogItems
                        //.Include(p => p.DependsOn)

                    join pbi in dbContext.ProductBacklogItems on p.ProductBacklogItemId equals pbi.Id
                    join prod in dbContext.Products on pbi.ProductId equals prod.Id

                    //join __pbi in dbContext.ProductBacklogItems on p.ProductBacklogItemId equals __pbi.Id into _pbi
                    //from pbi in _pbi.DefaultIfEmpty()

                    join __s in dbContext.Sprints on pbi.SprintId equals __s.Id into _s
                    from s in _s.DefaultIfEmpty()

                    where p.Id == sprintBacklogItemId
                    select new
                    {
                        p.Id,
                        p.ProductBacklogItemId,
                        ProductBacklogItemName = pbi.Name,
                        //ProductId = prod.Id,
                        //ProductName = prod.Name,
                        p.Name,
                        p.Description,
                        p.EstimationPoints,
                        p.Status,
                        p.Notes,
                        SprintId = s == null ? (Guid?)null : s.Id,
                        SprintName = s == null ? null : s.Name,
                        //DependsOn = p.DependsOn.Select(d => d.Id)
                    };

        var entity = await query
            .SingleOrDefaultAsync(ct);

        return entity is null
            ? null
            : new(entity.Id, entity.ProductBacklogItemId, entity.ProductBacklogItemName,
            //entity.ProductId, entity.ProductName,
            entity.Name, entity.Description,
            entity.Notes,
            entity.SprintId,
            entity.SprintName,
            entity.EstimationPoints, (SprintBacklogItemStatusDto)entity.Status);
        //entity.DependsOn.ToList());
    }
    public async Task<List<SprintBacklogItemListDto>> GetSprintBacklogItemsAsync(
        Guid? pbiFilter,
        Guid? goalFilter,
        string? nameFilter,
        CancellationToken ct)
    {
        var query = from p in dbContext.SprintBacklogItems
                    join bi in dbContext.ProductBacklogItems on p.ProductBacklogItemId equals bi.Id

                    join __inc in dbContext.Sprints on bi.SprintId equals __inc.Id into _inc
                    from inc in _inc.DefaultIfEmpty()

                    select new
                    {
                        p.Id,
                        p.Name,

                        p.ProductBacklogItemId,
                        ProductBacklogItemName = bi.Name,
                        p.Status,
                        p.EstimationPoints,
                        GoalId = inc == null ? (Guid?)null : inc.Id,
                        GoalName = inc == null ? null : inc.Name
                    };

        if (goalFilter is not null)
        {
            query = from q in query
                    where q.GoalId == goalFilter
                    select q;
        }

        if (pbiFilter is not null)
        {
            query = from q in query
                    where q.ProductBacklogItemId == pbiFilter
                    select q;
        }

        if (nameFilter is not null)
        {
            query = query.Where(s => s.Name.StartsWith(nameFilter));
        }

        var entities = await query
            .ToListAsync(ct);

        return entities.Select(e =>
            new SprintBacklogItemListDto(e.Id, e.Name, e.ProductBacklogItemName,
                e.EstimationPoints,
                (SprintBacklogItemStatusDto)e.Status,
                e.GoalName)).ToList();
    }
}
