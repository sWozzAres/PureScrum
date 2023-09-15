using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Scrum.Api.Domain.Infrastructure;
using Scrum.Shared.Helpers;
using Scrum.Web.Api.Helpers;
using ScrumApi;
using static ScrumApi.ProductBacklogItemService;

namespace Scrum.Web.Api.Services;

[Authorize(Policy = "ClientPolicy")]
public class ProductBacklogItemService(ScrumDbContext dbContext, ILogger<ProductBacklogItemService> logger) : ProductBacklogItemServiceBase
{
    static readonly string[] ValidListFilters = ["ProductId", "SprintId", "ActiveOnly"];

    public override async Task<ListProductBacklogItemsResponse> List(ListProductBacklogItemsRequest request, ServerCallContext context)
    {
        var filters = ApiHelpers.ParseStringToDictionary(request.Filter)
            ?? throw new RpcException(new Status(StatusCode.FailedPrecondition, "Invalid filter."));

        // validate filter keys
        if (filters.Keys.Except(ValidListFilters).Any())
            throw new RpcException(new Status(StatusCode.FailedPrecondition, "Unsupported filter."));

        // base query
        var baseQuery = from pbi in dbContext.ProductBacklogItems
                    .Include(x => x.SprintBacklogItems).AsSplitQuery()
                    .Include(x => x.Children).AsSplitQuery()

                        join __s in dbContext.Sprints on pbi.SprintId equals __s.Id into _s
                        from s in _s.DefaultIfEmpty()

                        join p in dbContext.Products on pbi.ProductId equals p.Id

                        select new { pbi, s, p };

        // filters
        if (filters.TryGetValue("ProductId", out string? productId))
        {
            if (!Guid.TryParse(productId, out var productIdAsGuid))
                throw new RpcException(new Status(StatusCode.FailedPrecondition, "Invalid ProductId filter."));

            baseQuery = baseQuery.Where(x => x.p.Id == productIdAsGuid);
        }

        if (filters.TryGetValue("SprintId", out string? sprintId))
        {
            if (!Guid.TryParse(sprintId, out var sprintIdAsGuid))
                throw new RpcException(new Status(StatusCode.FailedPrecondition, "Invalid SprintId filter."));

            baseQuery = baseQuery.Where(x => x.s.Id == sprintIdAsGuid);
        }

        if (filters.TryGetValue("ActiveOnly", out string? activeOnly))
        {
            if (!bool.TryParse(activeOnly, out var activeOnlyAsBool))
                throw new RpcException(new Status(StatusCode.FailedPrecondition, "Invalid ActiveOnly filter."));

            if (activeOnlyAsBool)
                baseQuery = baseQuery.Where(x => x.pbi.Status == Scrum.Api.Domain.PbiStatus.None || x.pbi.Status == Scrum.Api.Domain.PbiStatus.Ready);
        }

        // get anonymous type
        var query = from q in baseQuery
                    select new PbiTemp
                    (
                        q.pbi.Id,
                        q.pbi.Name,
                        (PbiStatus)q.pbi.Status,
                        q.s == null ? "" : q.s.Name,
                        q.p.Name,
                        q.pbi.EstimationPoints,
                        q.pbi.DeliveryDate,
                        q.pbi.Value,
                        q.pbi.Roi,
                        request.IncludeSbis
                        ? q.pbi.SprintBacklogItems.Select(sbi => new SbiShortTemp(
                            sbi.Id,
                            sbi.Name,
                            (SbiStatus)sbi.Status
                        ))
                        : null,
                        request.IncludeDependsOn
                        ? q.pbi.Children.Select(pbi => new PbiShortTemp(
                            pbi.Id,
                            pbi.Name,
                            (PbiStatus)pbi.Status
                        ))
                        : null
                    );

        var result = await query.ToListAsync(context.CancellationToken);

        List<Guid> allMissing = [];

        // load all pbis that are depended on, but not included in the original query
        if (request.IncludeDependsOn)
        {
            allMissing = await GetAllMissingPbis(result, (missing, cancellationToken) =>
            {
                var missingPbis = from pbi in dbContext.ProductBacklogItems
                        .Include(x => x.SprintBacklogItems).AsSplitQuery()
                        .Include(x => x.Children).AsSplitQuery()

                                  join __s in dbContext.Sprints on pbi.SprintId equals __s.Id into _s
                                  from s in _s.DefaultIfEmpty()

                                  join p in dbContext.Products on pbi.ProductId equals p.Id

                                  where missing.Contains(pbi.Id)
                                  select new PbiTemp
                                      (
                                          pbi.Id,
                                          pbi.Name,
                                          (PbiStatus)pbi.Status,
                                          s == null ? "" : s.Name,
                                          p.Name,
                                          pbi.EstimationPoints,
                                          pbi.DeliveryDate,
                                          pbi.Value,
                                          pbi.Roi,
                                          request.IncludeSbis
                                          ? pbi.SprintBacklogItems.Select(sbi => new SbiShortTemp(
                                              sbi.Id,
                                              sbi.Name,
                                              (SbiStatus)sbi.Status
                                          ))
                                          : null,
                                          request.IncludeDependsOn
                                          ? pbi.Children.Select(pbi => new PbiShortTemp(
                                              pbi.Id,
                                              pbi.Name,
                                              (PbiStatus)pbi.Status
                                          ))
                                          : null
                                      );

                return missingPbis.ToListAsync(cancellationToken);
            }
            , context.CancellationToken);
        }

        var response = new ListProductBacklogItemsResponse();

        foreach (var p in result)
        {
            var pbi = new ProductBacklogItem()
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Status = p.Status,
                SprintName = p.SprintName,
                ProductName = p.ProductName,
                EstimationPoints = p.EstimationPoints,
                DeliveryDate = p.DeliveryDate.FromDateOnly(),
                Value = p.Value,
                Roi = p.Roi,
                WasMissing = allMissing.Exists(m => m == p.Id)
            };

            if (p.SprintBacklogItems is not null)
                pbi.SprintBacklogItems.AddRange(p.SprintBacklogItems.Select(x => new SprintBacklogItemShort()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Status = x.Status,
                }));

            if (p.DependentOn is not null)
                pbi.DependentOn.AddRange(p.DependentOn.Select(x => new ProductBacklogItemShort()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Status = x.Status
                }));

            response.ProductBacklogItems.Add(pbi);
        }

        return response;

    }

    static async Task<List<Guid>> GetAllMissingPbis(
        List<PbiTemp> list,
        Func<HashSet<Guid>, CancellationToken, Task<List<PbiTemp>>> loadMissing,
        CancellationToken cancellationToken)
    {
        int loopCounter = 0;
        var allMissingIds = new List<Guid>();

        while (true)
        {
            loopCounter++;
            if (loopCounter > 20)
            {
                // infinite loop protection - this should never happen
                throw new InvalidOperationException("Recursion limit exceeded.");
            }

            var missingIds = GetMissingIds(list);

            if (missingIds.Count == 0)
                break;

            allMissingIds.AddRange(missingIds);

            var load = await loadMissing(missingIds, cancellationToken);
            if (load.Count != missingIds.Count)
            {
                throw new InvalidOperationException("Failed to load all missing objects.");
            }
            list.AddRange(load);
        }

        return allMissingIds;
    }

    static HashSet<Guid> GetMissingIds(List<PbiTemp> entities)
    {
        var missing = new HashSet<Guid>();
        var topLevelIds = entities.Select(e => e.Id);

        foreach (var entity in entities)
        {
            // DependentOn will never be null here because dependents are always loaded if we run
            // this method.
            foreach (var child in entity.DependentOn!)
            {
                if (!topLevelIds.Contains(child.Id))
                {
                    missing.Add(child.Id);
                }
            }
        }

        return missing;
    }

    record PbiTemp(Guid Id, string Name, PbiStatus Status, string SprintName, string ProductName,
        float EstimationPoints, DateOnly? DeliveryDate, int Value, int Roi,
        IEnumerable<SbiShortTemp>? SprintBacklogItems,
        IEnumerable<PbiShortTemp>? DependentOn);

    record SbiShortTemp(Guid Id, string Name, SbiStatus Status);
    record PbiShortTemp(Guid Id, string Name, PbiStatus Status);
}
