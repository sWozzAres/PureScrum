using Scrum.Api.Exceptions;

namespace Scrum.Api.Application.Commands.SprintUseCase;

public class UpdateSprint(
    ScrumDbContext dbContext,
    ILogger<UpdateSprint> logger) : UseCaseBase<Sprint>(dbContext)
{
    public async Task<bool> UpdateAsync(Guid sprintId, UpdateSprintRequest request, CancellationToken ct)
    {
        logger.LogInformation("Updating sprint {sprintid}, request {@updatesprintrequest}.",
            sprintId, request);

        await InitializeAndLoadAsync(sprintId, ct);

        Entity.SprintGoal = request.SprintGoal;
        Entity.ExpectedDeliveryDate = request.ExpectedDeliveryDate;
        Entity.Status = (BacklogStatus)request.Status;
        Entity.Name = request.Name;
        Entity.Started = request.Started;

        if (Entity.Started > Entity.ExpectedDeliveryDate)
            throw new ScrumDomainException("The sprint cannot start after the expected delivery date.");

        return DbContext.Entry(Entity).State == EntityState.Modified;
    }
}
