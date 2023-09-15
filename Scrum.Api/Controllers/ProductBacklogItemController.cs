using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Scrum.Api.Application.Commands.ProductBacklogItemUseCase;
using Scrum.Api.Application.Queries;
using Scrum.Api.Domain.Configuration;
using Scrum.Api.Exceptions;

namespace Scrum.Api.Controllers;
[Authorize(Policy = "ClientPolicy")]
[Route("api/[controller]")]
[ApiController]
public class ProductBacklogItemController(ScrumDbContext dbContext, ILogger<ProductBacklogItemController> logger) : ControllerBase
{
    static ScrumDomainException DuplicateNameException(string name)
        => new($"A product backlog item with the name '{name}' already exists.");

    const string OpenCacheKey = "pbi_open";

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateProductBacklogItem(
        CreateProductBacklogItemRequest request,
        [FromServices] CreateProductBacklogItem command,
        [FromServices] IMemoryCache cache,
        CancellationToken ct)
    {
        var item = command.Create(request);
        try
        {
            await dbContext.SaveChangesAsync(ct);


            cache.Remove(OpenCacheKey);

            //TODO can this be better??

            // load product, to get Name for returning in result
            var product = await dbContext.Products.FindAsync(new object[] { item.ProductId }, ct);

            return CreatedAtAction(nameof(GetProductBacklogItem), new { id = item.Id },
                new ProductBacklogItemFullDto(item.Id, item.ProductId,
                product is null ? "" : product.Name,
                (ProductBacklogItemStatusDto)item.Status,
                item.Name, item.Description, item.EstimationPoints, item.Notes, item.DeliveryDate, item.IsFixedDeliveryDate,
                item.Value, item.Roi, null, null,
                Enumerable.Empty<ProductBacklogItemDependentDto>().ToList(),
                Enumerable.Empty<SprintBacklogItemShortDto>().ToList()));
        }
        catch (DbUpdateException dbe)
            when (dbe.InnerException is SqlException se && se.Message.Contains(ProductBacklogItemEntityTypeConfiguration.UQ_ProductBacklogItems_ProductId_Name))
        {
            throw DuplicateNameException(request.Name);
        }
    }
    [HttpPost]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateProductBacklogItem(
        Guid id,
        [FromBody] UpdateProductBacklogItemRequest request,
        [FromServices] UpdateProductBacklogItem command,
        [FromServices] ProductBacklogItemQueries queries,
        [FromServices] IMemoryCache cache,
        CancellationToken ct)
    {
        if (await command.UpdateAsync(id, request, ct))
        {
            try
            {
                await dbContext.SaveChangesAsync(ct);

                cache.Remove(OpenCacheKey);
            }
            catch (DbUpdateException dbe)
                when (dbe.InnerException is SqlException se && se.Message.Contains(ProductBacklogItemEntityTypeConfiguration.UQ_ProductBacklogItems_ProductId_Name))
            {
                throw DuplicateNameException(request.Name!);
            }
        }

        return Ok(await queries.GetProductBacklogItemAsync(id, ct));
    }
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetProductBacklogItem(
        Guid id,
        [FromServices] ProductBacklogItemQueries queries,
        CancellationToken ct)
    {
        var pbi = await queries.GetProductBacklogItemAsync(id, ct);
        return pbi is null ? NotFound() : Ok(pbi);
    }

    [HttpGet]
    [Route("open")]
    public async Task<IActionResult> GetProductBacklogItemsOpen(
        [FromServices] ProductBacklogItemQueries queries,
        [FromServices] IMemoryCache cache,
        CancellationToken ct)
    {
        if (cache.TryGetValue(OpenCacheKey, out List<ProductBacklogItemListDto>? result))
        {
            logger.LogDebug("Found in cache.");
            return Ok(result);
        }

        logger.LogDebug("Not found in cache.");
        result = await queries.GetProductBacklogItemsAllAsync(null, null, true, ct);
        cache.Set(OpenCacheKey, result);

        return Ok(result);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetProductBacklogItems(
        [FromQuery(Name = "nf")] string? nameFilter,
        [FromServices] ProductBacklogItemQueries queries,
        CancellationToken ct)
    => Ok(await queries.GetProductBacklogItemsAsync(null, null, nameFilter, null, ct));

    [HttpPost]
    [Route("{id:guid}/dependencies")]
    public async Task<IActionResult> CreateDependencies(
        Guid id,
        [FromBody] List<Guid> dependencies,
        [FromServices] AddProductBacklogItemDependencies command,
        [FromServices] IMemoryCache cache,
        CancellationToken ct)
    {
        command.Add(id, dependencies);
        await dbContext.SaveChangesAsync(ct);

        cache.Remove(OpenCacheKey);

        return Ok();
    }

    [HttpDelete]
    [Route("{id:guid}/dependencies/{rid:guid}")]
    public async Task<IActionResult> RemoveDependency(
        Guid id,
        Guid rid,
        [FromServices] RemoveProductBacklogItemDependency command,
        [FromServices] IMemoryCache cache,
        CancellationToken ct)
    {
        var request = new RemoveDependencyRequest() { ProductBacklogItemId = rid };
        command.Remove(id, request);
        await dbContext.SaveChangesAsync(ct);

        cache.Remove(OpenCacheKey);

        return Ok();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteProductBacklogItem(
        Guid id,
        [FromServices] DeleteProductBacklogItem command,
        [FromServices] IMemoryCache cache,
        CancellationToken ct)
    {
        var deleted = await command.DeleteAsync(id, ct);
        if (deleted)
            await dbContext.SaveChangesAsync(ct);

        cache.Remove(OpenCacheKey);

        return deleted ? Ok() : NotFound();
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteMany(
        [FromBody] List<Guid> ids,
        [FromServices] DeleteProductBacklogItem command,
        [FromServices] IMemoryCache cache,
        CancellationToken ct)
    {
        foreach (var id in ids)
        {
            if (!await command.DeleteAsync(id, ct))
                throw new ScrumDomainException("Failed to find product backlog item.");
        }

        try
        {
            await dbContext.SaveChangesAsync(ct);

            cache.Remove(OpenCacheKey);
        }
        catch (DbUpdateException dbe)
            when (dbe.InnerException is SqlException se && se.Message.Contains(SprintBacklogItemEntityTypeConfiguration.FK_SprintBacklogItems_ProductBacklogItems_ProductBacklogItemsId))
        {
            throw new ScrumDomainException(ids.Count == 1
                ? "The product backlog item has one or more SBIs, so it cannot be deleted."
                : "At least one of the product backlog items contain one or more SBIs, so the item cannot be deleted.");
        }

        return Ok();
    }
    //[HttpGet]
    //[Route("{sprintId:guid}/all")]
    //public async Task<IActionResult> GetAllForSprint(
    //    Guid sprintId,
    //    [FromServices] ProductBacklogItemQueries queries,
    //    CancellationToken ct)
    //{
    //    return Ok(await queries.GetProductBacklogItemsAllAsync(sprintId, null, null, ct));
    //}
}
