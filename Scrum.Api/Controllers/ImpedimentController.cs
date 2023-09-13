using Microsoft.AspNetCore.Mvc;
using Scrum.Api.Application.Queries;
using Scrum.Api.Exceptions;

namespace Scrum.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImpedimentController(ScrumDbContext dbContext) : ControllerBase
{
    static ScrumDomainException DuplicateNameException(string name)
        => new($"An impediment with the name '{name}' already exists.");

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateImpediment(
        CreateImpedimentRequest request,
        //[FromServices] CreateImpediment command,
        CancellationToken ct)
    {
        var impediment = new Impediment(request.Name, request.Description, request.Severity, request.Value);
        dbContext.Impediments.Add(impediment);
        await dbContext.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetImpediment), new { id = impediment.Id }, new { id = impediment.Id });
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetImpediment(
        Guid id,
        [FromServices] ImpedimentQueries queries,
        CancellationToken ct)
    {
        var impediment = await queries.GetImpedimentAsync(id, ct);
        return impediment is null ? NotFound() : Ok(impediment);
    }

    [HttpPost]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateImpediment(
        Guid id,
        [FromBody] UpdateImpedimentRequest request,
        [FromServices] ImpedimentQueries queries,
        CancellationToken ct)
    {
        var impediment = await dbContext.Impediments.FindAsync(new object[] { id }, ct);
        if (impediment is null)
            return NotFound();

        impediment.Name = request.Name;
        impediment.Description = request.Description;
        impediment.Severity = request.Severity;
        impediment.Value = request.Value;

        if (dbContext.Entry(impediment).State == EntityState.Modified)
        {
            await dbContext.SaveChangesAsync(ct);
        }

        return Ok(await queries.GetImpedimentAsync(id, ct));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteImpediment(
        Guid id,
        //[FromServices] DeleteImpediment command,
        CancellationToken ct)
    {
        var impediment = await dbContext.Impediments.FindAsync(new object[] { id }, ct);
        if (impediment is null)
            return NotFound();

        dbContext.Impediments.Remove(impediment);

        await dbContext.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetImpediments(
        [FromServices] ImpedimentQueries queries,
        CancellationToken ct)
    => Ok(await queries.GetImpedimentsAsync(ct));

    [HttpDelete]
    public async Task<IActionResult> DeleteMany(
        [FromBody] List<Guid> ids,
        CancellationToken ct)
    {
        foreach (var id in ids)
        {
            var impediment = await dbContext.Impediments.FindAsync(new object[] { id }, ct)
                ?? throw new ScrumDomainException("Failed to find impediment.");
            dbContext.Impediments.Remove(impediment);
        }

        await dbContext.SaveChangesAsync(ct);

        return Ok();
    }
}
