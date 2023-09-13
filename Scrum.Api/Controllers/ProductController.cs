using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Scrum.Api.Application.Commands.ProductUseCase;
using Scrum.Api.Application.Queries;
using Scrum.Api.Domain.Configuration;
using Scrum.Api.Exceptions;

namespace Scrum.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(ScrumDbContext dbContext, ILogger<ProductController> logger) : ControllerBase
{
    static ScrumDomainException DuplicateNameException(string name)
        => new($"A product with the name '{name}' already exists.");

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateProduct(
        CreateProductRequest request,
        [FromServices] CreateProduct command,
        //[FromServices] ProductQueries queries,
        CancellationToken ct)
    {
        var product = command.Create(request);

        try
        {
            await dbContext.SaveChangesAsync(ct);

            //var result = await queries.GetProductAsync(product.Id, ct);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new ProductShortDto(product.Id, product.Name));
        }
        catch (DbUpdateException dbe)
            when (dbe.InnerException is SqlException se && se.Message.Contains(ProductEntityTypeConfiguration.UQ_Product_Name))
        {
            throw DuplicateNameException(request.Name);
        }
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetProduct(
        Guid id,
        [FromServices] ProductQueries queries,
        CancellationToken ct)
    {
        var product = await queries.GetProductAsync(id, ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductRequest request,
        [FromServices] UpdateProduct command,
        [FromServices] ProductQueries queries,
        CancellationToken ct)
    {
        if (await command.UpdateAsync(id, request, ct))
        {
            try
            {
                await dbContext.SaveChangesAsync(ct);
            }
            catch (DbUpdateException dbe)
                when (dbe.InnerException is SqlException se && se.Message.Contains(ProductEntityTypeConfiguration.UQ_Product_Name))
            {
                throw DuplicateNameException(request.Name);
            }
        }

        var product = await queries.GetProductAsync(id, ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromServices] ProductQueries queries,
        CancellationToken ct)
    => Ok(await queries.GetProductsAsync(ct));

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(
        Guid id,
        [FromServices] DeleteProduct command,
        CancellationToken ct)
    {
        var deleted = await command.DeleteAsync(id, ct);
        if (deleted)
            await dbContext.SaveChangesAsync(ct);

        return deleted ? Ok() : NotFound();
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteMany(
        [FromBody] List<Guid> ids,
        [FromServices] DeleteProduct command,
        CancellationToken ct)
    {
        foreach (var id in ids)
        {
            if (!await command.DeleteAsync(id, ct))
                throw new ScrumDomainException("Failed to find product.");
        }

        try
        {
            await dbContext.SaveChangesAsync(ct);
        }
        catch (DbUpdateException dbe)
            when (dbe.InnerException is SqlException se && se.Message.Contains(ProductBacklogItemEntityTypeConfiguration.FK_ProductBacklogItems_Products_ProductId))
        {
            throw new ScrumDomainException(ids.Count == 1
                ? "The product has one or more PBIs, so it cannot be deleted."
                : "At least one of the products contain one or more PBIs, so the products cannot be deleted.");
        }

        return Ok();
    }
    [HttpGet]
    [Route("{productId:guid}/productbacklogitems")]
    public async Task<IActionResult> GetProductBacklogItems(
        Guid productId,
        [FromServices] ProductBacklogItemQueries queries,
        CancellationToken ct)
    => Ok(await queries.GetProductBacklogItemsAllAsync(null, productId, null, ct));

    /// <summary>
    /// test
    /// see filtering https://google.aip.dev/160
    /// </summary>
    /// <param name="c"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    //[HttpGet]
    //[Route("custom")]
    //public async Task<IActionResult> GetProductsCustom(
    //    [FromQuery] string? c,
    //    CancellationToken ct)
    //{
    //    var columns = string.IsNullOrEmpty(c) 
    //        ? new string[] { "Id" }
    //        : c.Split(",");

    //    var sql = $"SELECT {string.Join(", ", columns)} " +
    //        "FROM Products ";

    //    var result = await dbContext.Database.GetDbConnection().QueryAsync(sql);

    //    logger.LogDebug("result {@result}", result);

    //    return Ok(result);
    //}
}
