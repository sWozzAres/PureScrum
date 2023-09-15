using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scrum.Api.Controllers;
[Authorize(Policy = "ClientPolicy")]
[Route("api/[controller]")]
[ApiController]
public class DataController(ScrumDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [Route("sprintpbis")]
    public async Task<IActionResult> SprintPBIs(
        CancellationToken ct)
    {
        var query = dbContext.Sprints
            .Select(s => new
            {
                s.Name,
                Count = s.BacklogItems.Count(),
                ActiveCount = s.BacklogItems.Count(x => x.Status != PbiStatus.Done)
            });

        var result = await query
            .Select(q => new SprintPBIs(q.Name, q.Count, q.ActiveCount))
            .ToListAsync(ct);

        return Ok(result);
    }

    //TODO remove
    [HttpGet]
    [Route("info")]
    public IActionResult GetInfo(IHttpContextAccessor a)//(ClaimsPrincipal claimsPrincipal)
    {
        var result = a.HttpContext?.User.Claims.Select(x => new ApiClaim(x.Type, x.Value));
        return Ok(result);
    }

    record ApiClaim(string Type, string Value);
}

