namespace Scrum.Api.Application.Queries;

public class SprintQueries(ScrumDbContext dbContext, ILogger<SprintQueries> logger)
{
    public async Task<SprintFullDto?> GetSprintAsync(Guid sprintId, CancellationToken ct)
    {
        var query = from p in dbContext.Sprints
                    where p.Id == sprintId
                    select new
                    {
                        p.Id,
                        p.SprintGoal,
                        p.Name,
                        p.ExpectedDeliveryDate,
                        p.Status,
                        p.Started
                    };

        var entity = await query
            .SingleOrDefaultAsync(ct);

        return entity is null
            ? null
            : new(entity.Id, entity.SprintGoal, entity.ExpectedDeliveryDate, (SprintStatusDto)entity.Status,
            entity.Name, entity.Started);
    }
    public async Task<List<SprintListDto>> GetSprintsAsync(CancellationToken ct)
    {
        var query = from p in dbContext.Sprints
                    select new
                    {
                        p.Id,
                        p.SprintGoal,
                        p.Name,
                        p.ExpectedDeliveryDate,
                        p.Status,
                    };

        var entities = await query
            .ToListAsync(ct);

        return entities.Select(e =>
            new SprintListDto(e.Id, e.SprintGoal,
                e.ExpectedDeliveryDate, (SprintStatusDto)e.Status,
                e.Name)).ToList();
    }
    /// <summary>
    /// Returns the burndown data for the specified sprint.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="utcDateTime"></param>
    /// <param name="ct"></param>
    /// <returns>null if the sprint was not found</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<List<BurndownEntry>?> GetBurndownData(
        Guid id,
        DateTime utcDateTime,
        DateTime? endUtcDateTime,
        DateTime? startUtcDateTime,
        CancellationToken ct)
    {
        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException("Supplied datetime must be of kind Utc.", nameof(utcDateTime));
        if (endUtcDateTime.HasValue && endUtcDateTime!.Value.Kind != DateTimeKind.Utc)
            throw new ArgumentException("Supplied datetime must be of kind Utc.", nameof(endUtcDateTime));
        if (startUtcDateTime.HasValue && startUtcDateTime!.Value.Kind != DateTimeKind.Utc)
            throw new ArgumentException("Supplied datetime must be of kind Utc.", nameof(startUtcDateTime));

        var sprint = await dbContext.Sprints.FindAsync(new object[] { id }, ct);
        if (sprint is null)
            return null;

        List<BurndownEntry> entries = new();

        DateTime now = utcDateTime.Date;

        // start defaults to sprint created date
        DateTime startUtc = startUtcDateTime is null
            ? sprint.Created.Date
            : startUtcDateTime.Value;

        // end default to now
        DateTime endUtc = endUtcDateTime is not null
            ? endUtcDateTime.Value
            : now;

        var currentUtc = startUtc;
        while (currentUtc <= endUtc)
        {
            //if (utcDateTime > current && utcDateTime < current.AddDays(1))
            //{
            //    logger.LogDebug("Doing now!!");
            //    await GetDataFor(utcDateTime);

            //}

            if (currentUtc > now)
            {
                entries.Add(new(currentUtc, null, null, null));
                currentUtc = currentUtc.AddHours(24);
                continue;
            }

            await GetDataFor(currentUtc);
            currentUtc = currentUtc.AddHours(24);

            async Task GetDataFor(DateTime asOf)
            {
                var xquery = from pbi in dbContext.ProductBacklogItems.TemporalAsOf(asOf)
                             where pbi.SprintId == id
                             group pbi by pbi.SprintId into g
                             select new
                             {
                                 NonePoints = g.Where(x => x.Status == PbiStatus.None).Sum(x => x.EstimationPoints),
                                 ReadyPoints = g.Where(x => x.Status == PbiStatus.Ready).Sum(x => x.EstimationPoints),
                                 DonePoints = g.Where(x => x.Status == PbiStatus.Done).Sum(x => x.EstimationPoints),
                             };

                var xtotal = await xquery.SingleOrDefaultAsync(ct);
                if (xtotal is null)
                {
                    entries.Add(new(asOf, null, null, null));
                }
                else
                {
                    entries.Add(new(asOf, xtotal.NonePoints, xtotal.ReadyPoints, xtotal.DonePoints));
                }
            }
        }

        return entries;
    }
}
