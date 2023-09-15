using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Scrum.Api.Application.Commands.SprintUseCase;
using Scrum.Api.Application.Queries;
using Scrum.Api.Domain.Configuration;
using Scrum.Api.Exceptions;

namespace Scrum.Api.Controllers;
[Authorize(Policy = "ClientPolicy")]
[Route("api/[controller]")]
[ApiController]
public class SprintController(ScrumDbContext dbContext, ILogger<SprintController> logger) : ControllerBase
{
    static ScrumDomainException DuplicateNameException(string name)
        => new($"A sprint goal with the name '{name}' already exists.");

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateSprint(
        CreateSprintRequest request,
        [FromServices] CreateSprint command,
        CancellationToken ct)
    {
        var sprint = command.Create(request);
        try
        {
            await dbContext.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(GetSprint), new { id = sprint.Id }, new SprintShortDto(sprint.Id, sprint.Name));
        }
        catch (DbUpdateException dbe)
            when (dbe.InnerException is SqlException se && se.Message.Contains(SprintEntityTypeConfiguration.UQ_Sprints_Name))
        {
            throw DuplicateNameException(request.Name);
        }
    }
    [HttpPost]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateSprint(
        Guid id,
        [FromBody] UpdateSprintRequest request,
        [FromServices] UpdateSprint command,
        [FromServices] SprintQueries queries,
        CancellationToken ct)
    {
        if (await command.UpdateAsync(id, request, ct))
        {
            try
            {
                await dbContext.SaveChangesAsync(ct);
            }
            catch (DbUpdateException dbe)
                when (dbe.InnerException is SqlException se && se.Message.Contains(SprintEntityTypeConfiguration.UQ_Sprints_Name))
            {
                throw DuplicateNameException(request.Name);
            }
        }

        return Ok(await queries.GetSprintAsync(id, ct));
    }
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetSprint(
        Guid id,
        [FromServices] SprintQueries queries,
        CancellationToken ct)
    {
        var sprint = await queries.GetSprintAsync(id, ct);
        return sprint is null ? NotFound() : Ok(sprint);
    }

    [HttpGet]
    public async Task<IActionResult> GetSprints(
        [FromServices] SprintQueries queries,
        CancellationToken ct)
    => Ok(await queries.GetSprintsAsync(ct));

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteSprint(
        Guid id,
        [FromServices] DeleteSprint command,
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
        [FromServices] DeleteSprint command,
        CancellationToken ct)
    {
        foreach (var id in ids)
        {
            await command.DeleteAsync(id, ct);
        }

        await dbContext.SaveChangesAsync(ct);

        return Ok();
    }

    /// <summary>
    /// Returns the burndown data for the specified sprint up to the specified date.
    /// </summary>
    /// <param name="id">The sprint id</param>
    /// <param name="utcDateTime">The date in UTC</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">If a date parameter was invalid</exception>
    [HttpGet]
    [Route("{id:guid}/burndown")]
    public async Task<IActionResult> GetBurndownData(
        Guid id,
        [FromServices] SprintQueries queries,
        [FromQuery(Name = "d")] string utcDateTime,
        [FromQuery(Name = "e")] string? endUtcDateTime,
        [FromQuery(Name = "s")] string? startUtcDateTime,
        [FromServices] IMemoryCache memoryCache,
        CancellationToken ct)
    {
        logger.LogDebug("d,e = {d},{e}", utcDateTime, endUtcDateTime);

        ParseUtcDateOrThrow(utcDateTime, out DateTime now);

        string key = $"burndown-{id}-{now}";

        DateTime? start = null;
        DateTime? end = null;

        if (startUtcDateTime is not null)
        {
            ParseUtcDateOrThrow(startUtcDateTime, out DateTime parsedDate);

            start = parsedDate;
            key += $"-{start}";
        }

        if (endUtcDateTime is not null)
        {
            ParseUtcDateOrThrow(endUtcDateTime, out DateTime parsedDate);

            end = parsedDate;
            key += $"-{end}";
        }


        if (!memoryCache.TryGetValue(key, out List<BurndownEntry>? entries))
        {
            logger.LogDebug("Burndown data is not in the cache. Key '{key}'.", key);

            entries = await queries.GetBurndownData(id, now, end, start, ct);

            if (entries is not null)
            {
                memoryCache.Set(key, entries, TimeSpan.FromDays(1));
                return Ok(entries);
            }
            else
            {
                return NotFound();
            }
        }

        logger.LogDebug("Found burndown data in the cache. Key '{key}'.", key);
        return Ok(entries);

        static void ParseUtcDateOrThrow(string utcDateString, out DateTime utcDate)
        {
            if (!DateTime.TryParse(utcDateString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out utcDate))
                throw new ArgumentException("Invalid date parameter.", nameof(utcDateString));
        }
    }
    [HttpGet]
    [Route("{sprintId:guid}/productbacklogitems")]
    public async Task<IActionResult> GetSprintBacklogItems(
        Guid sprintId,
        [FromServices] ProductBacklogItemQueries queries,
        CancellationToken ct)
    => Ok(await queries.GetProductBacklogItemsAllAsync(sprintId, null, null, ct));
}
