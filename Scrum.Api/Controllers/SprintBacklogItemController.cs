using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Scrum.Api.Application.Commands.SprintBacklogItemUseCase;
using Scrum.Api.Application.Commands.SprintUseCase;
using Scrum.Api.Application.Queries;
using Scrum.Api.Domain.Configuration;
using Scrum.Api.Exceptions;

namespace Scrum.Api.Controllers;
[Authorize(Policy = "ClientPolicy")]
[Route("api/[controller]")]
[ApiController]
public class SprintBacklogItemController(ScrumDbContext dbContext) : ControllerBase
{
    static ScrumDomainException DuplicateNameException(string name)
        => new($"A sprint backlog item with the name '{name}' already exists.");

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateSprintBacklogItem(
        CreateSprintBacklogItemRequest request,
        [FromServices] CreateSprintBacklogItem command,
        CancellationToken ct)
    {
        var increment = command.Create(request);
        try
        {
            await dbContext.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(GetSprintBacklogItem), new { id = increment.Id }, new { id = increment.Id });
        }
        catch (DbUpdateException dbe)
            when (dbe.InnerException is SqlException se && se.Message.Contains(SprintBacklogItemEntityTypeConfiguration.UQ_SprintBacklogItems_ProductBacklogItemId_Name))
        {
            throw DuplicateNameException(request.Name);
        }
    }
    [HttpPost]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateSprintBacklogItem(
        Guid id,
        [FromBody] UpdateSprintBacklogItemRequest request,
        [FromServices] UpdateSprintBacklogItem command,
        [FromServices] SprintBacklogItemQueries queries,
        CancellationToken ct)
    {
        if (await command.UpdateAsync(id, request, ct))
        {
            try
            {
                await dbContext.SaveChangesAsync(ct);
            }
            catch (DbUpdateException dbe)
                when (dbe.InnerException is SqlException se && se.Message.Contains(SprintBacklogItemEntityTypeConfiguration.UQ_SprintBacklogItems_ProductBacklogItemId_Name))
            {
                throw DuplicateNameException(request.Name!);
            }
        }

        return Ok(await queries.GetSprintBacklogItemAsync(id, ct));
    }
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetSprintBacklogItem(
        Guid id,
        [FromServices] SprintBacklogItemQueries queries,
        CancellationToken ct)
    {
        var sprintBacklogItem = await queries.GetSprintBacklogItemAsync(id, ct);
        return sprintBacklogItem is null ? NotFound() : Ok(sprintBacklogItem);
    }

    [HttpGet]
    public async Task<IActionResult> GetSprintBacklogItems(
        [FromQuery] Guid? g,
        [FromQuery] Guid? pbi,
        [FromQuery(Name = "nf")] string? nameFilter,
        [FromServices] SprintBacklogItemQueries queries,
        CancellationToken ct)
    => Ok(await queries.GetSprintBacklogItemsAsync(pbi, g, nameFilter, ct));

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteSprintBacklogItemBacklogItem(
        Guid id,
        [FromServices] DeleteSprintBacklogItem command,
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
        [FromServices] DeleteSprintBacklogItem command,
        CancellationToken ct)
    {
        foreach (var id in ids)
        {
            await command.DeleteAsync(id, ct);
        }

        await dbContext.SaveChangesAsync(ct);

        return Ok();
    }
    [HttpPost]
    [Route("{sbiId:guid}/move")]
    public async Task<IActionResult> Move(
        Guid sbiId,
        MoveSbi request,
        [FromServices] MoveToPbi command,
        CancellationToken ct)
    {
        await command.MoveAsync(sbiId, request, ct);
        await dbContext.SaveChangesAsync(ct);

        return Ok();
    }

}
