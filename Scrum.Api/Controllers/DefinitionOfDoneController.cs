using Microsoft.AspNetCore.Mvc;
using Scrum.Api.Application.Queries;
using Scrum.Api.Exceptions;

namespace Scrum.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DefinitionOfDoneController(ScrumDbContext dbContext) : ControllerBase
{
    static ScrumDomainException DuplicateNameException(string name)
        => new($"An definition of done with the name '{name}' already exists.");

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateDefinitionOfDone(
        CreateDefinitionOfDoneRequest request,
        //[FromServices] CreateDefinitionOfDone command,
        CancellationToken ct)
    {
        var definitionofdone = new DefinitionOfDone(request.Name, request.Description);
        dbContext.DefinitionOfDones.Add(definitionofdone);
        await dbContext.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetDefinitionOfDone), new { id = definitionofdone.Id }, new { id = definitionofdone.Id });
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetDefinitionOfDone(
        Guid id,
        [FromServices] DefinitionOfDoneQueries queries,
        CancellationToken ct)
    {
        var definition = await queries.GetDefinitionOfDoneAsync(id, ct);
        return definition is null ? NotFound() : Ok(definition);
    }

    [HttpPost]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateDefinitionOfDone(
        Guid id,
        [FromBody] UpdateDefinitionOfDoneRequest request,
        [FromServices] DefinitionOfDoneQueries queries,
        CancellationToken ct)
    {
        var definitionofdone = await dbContext.DefinitionOfDones.FindAsync(new object[] { id }, ct);
        if (definitionofdone is null)
            return NotFound();

        definitionofdone.Name = request.Name;
        definitionofdone.Description = request.Description;

        if (dbContext.Entry(definitionofdone).State == EntityState.Modified)
        {
            await dbContext.SaveChangesAsync(ct);
        }

        return Ok(await queries.GetDefinitionOfDoneAsync(id, ct));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteDefinitionOfDone(
        Guid id,
        //[FromServices] DeleteDefinitionOfDone command,
        CancellationToken ct)
    {
        var definitionofdone = await dbContext.DefinitionOfDones.FindAsync(new object[] { id }, ct);
        if (definitionofdone is null)
            return NotFound();

        dbContext.DefinitionOfDones.Remove(definitionofdone);

        await dbContext.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetDefinitionOfDones(
        [FromServices] DefinitionOfDoneQueries queries,
        CancellationToken ct)
    => Ok(await queries.GetDefinitionOfDonesAsync(ct));

    [HttpDelete]
    public async Task<IActionResult> DeleteMany(
        [FromBody] List<Guid> ids,
        CancellationToken ct)
    {
        foreach (var id in ids)
        {
            var definitionofdone = await dbContext.DefinitionOfDones.FindAsync(new object[] { id }, ct)
                ?? throw new ScrumDomainException("Failed to find definitionofdone.");
            dbContext.DefinitionOfDones.Remove(definitionofdone);
        }

        await dbContext.SaveChangesAsync(ct);

        return Ok();
    }
}
