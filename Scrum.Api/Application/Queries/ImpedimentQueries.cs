namespace Scrum.Api.Application.Queries;

public class ImpedimentQueries(ScrumDbContext dbContext)
{
    public async Task<ImpedimentFullDto?> GetImpedimentAsync(Guid impedimentId, CancellationToken ct)
    {
        var query = from p in dbContext.Impediments
                    where p.Id == impedimentId
                    select new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.Severity,
                        p.Value
                    };

        var entity = await query
            .SingleOrDefaultAsync(ct);

        return entity is null
            ? null
            : new(entity.Id, entity.Name, entity.Description, entity.Severity, entity.Value);
    }
    public async Task<List<ImpedimentListDto>> GetImpedimentsAsync(CancellationToken ct)
    {
        var query = from p in dbContext.Impediments
                    select new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.Severity,
                        p.Value
                    };

        var entities = await query
            .ToListAsync(ct);

        return entities.Select(e => new ImpedimentListDto(e.Id, e.Name, e.Description, e.Severity, e.Value)).ToList();
    }
}
