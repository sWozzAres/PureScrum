using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Scrum.Api.Domain.Infrastructure;
using ScrumApi;
using static ScrumApi.SprintService;

namespace Scrum.Web.Api.Services;

public class SprintService(ScrumDbContext dbContext) : SprintServiceBase
{
    public override async Task<ListSprintsResponse> List(ListSprintsRequest request, ServerCallContext context)
    {
        var query = from p in dbContext.Sprints
                    select new SprintShort()
                    {
                        Id = p.Id.ToString(),
                        Name = p.Name,
                        ExpectedDeliveryDate = Timestamp.FromDateTimeOffset(p.ExpectedDeliveryDate ?? DateTimeOffset.MaxValue),
                        ExpectedDeliveryDateIsValid = p.ExpectedDeliveryDate != null,
                        Status = (int)p.Status
                    };

        var response = new ListSprintsResponse();
        response.Sprints.AddRange(await query.ToListAsync(context.CancellationToken));

        return response;
    }
}
