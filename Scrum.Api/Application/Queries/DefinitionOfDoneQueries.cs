namespace Scrum.Api.Application.Queries;

public class DefinitionOfDoneQueries(ScrumDbContext dbContext)
{
    public async Task<DefinitionOfDoneFullDto?> GetDefinitionOfDoneAsync(Guid DefinitionOfDoneId, CancellationToken ct)
    {
        var query = from p in dbContext.DefinitionOfDones
                    where p.Id == DefinitionOfDoneId
                    select new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                    };

        var entity = await query
            .SingleOrDefaultAsync(ct);

        return entity is null
            ? null
            : new(entity.Id, entity.Name, entity.Description);
    }
    public async Task<List<DefinitionOfDoneListDto>> GetDefinitionOfDonesAsync(CancellationToken ct)
    {
        var query = from p in dbContext.DefinitionOfDones
                    select new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                    };

        var entities = await query
            .ToListAsync(ct);

        return entities.Select(e => new DefinitionOfDoneListDto(e.Id, e.Name, e.Description)).ToList();
    }
}
