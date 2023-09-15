using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Scrum.Api.Domain.Infrastructure;
using ScrumApi;
using static ScrumApi.SprintBacklogItemService;

namespace Scrum.Web.Api.Services;

[Authorize(Policy = "ClientPolicy")]
public class SprintBacklogItemService(ScrumDbContext dbContext) : SprintBacklogItemServiceBase
{
    
    public override async Task<ListSprintBacklogItemsResponse> List(ListSprintBacklogItemsRequest request, ServerCallContext context)
    {
        var query = from p in dbContext.SprintBacklogItems.AsNoTracking()
                    join bi in dbContext.ProductBacklogItems on p.ProductBacklogItemId equals bi.Id

                    join __inc in dbContext.Sprints on bi.SprintId equals __inc.Id into _inc
                    from inc in _inc.DefaultIfEmpty()

                    select new SprintBacklogItemListShort()
                    {
                        Id = p.Id.ToString(),
                        Name = p.Name,

                        ProductBacklogItemId = p.ProductBacklogItemId.ToString(),
                        ProductBacklogItemName = bi.Name,
                        Status = (SbiStatus)p.Status,
                        EstimationPoints = p.EstimationPoints,

                        SprintId = inc == null ? "" : inc.Id.ToString(),
                        SprintName = inc == null ? "" : inc.Name
                    };

        var response = new ListSprintBacklogItemsResponse();
        response.SprintBacklogItems.AddRange(await query.ToListAsync(context.CancellationToken));

        return response;
    }
}
