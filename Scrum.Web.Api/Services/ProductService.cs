using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Scrum.Api.Domain.Infrastructure;
using ScrumApi;
using static ScrumApi.ProductService;

namespace Scrum.Web.Api.Services;

[Authorize(Policy = "ClientPolicy")]
public class ProductService(ScrumDbContext dbContext) : ProductServiceBase
{
    public override async Task<ListProductsResponse> List(ListProductsRequest request, ServerCallContext context)
    {
        //await Task.Delay(3000);
        //throw new RpcException(new Status(StatusCode.FailedPrecondition, "test"));
        var query = from p in dbContext.Products
                    select new ProductShort()
                    {
                        Id = p.Id.ToString(),
                        Name = p.Name,
                    };

        var response = new ListProductsResponse();
        response.Products.AddRange(await query.ToListAsync(context.CancellationToken));

        return response;
    }

    public override Task<CreateResponse> Create(
        CreateRequest request, 
        ServerCallContext context)
    {
        return base.Create(request, context);
    }
}
