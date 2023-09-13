namespace Scrum.Api.Application.Commands.SprintBacklogItemUseCase;

public class UpdateSprintBacklogItem(
    ScrumDbContext dbContext,
    ILogger<UpdateSprintBacklogItem> logger) : UseCaseBase<SprintBacklogItem>(dbContext)
{
    public async Task<bool> UpdateAsync(Guid sprintBacklogItemId, UpdateSprintBacklogItemRequest request, CancellationToken ct)
    {
        logger.LogInformation("Updating sprint backlog item {sprintBacklogItemid}, request {@updatesprintbacklogitemrequest}.",
            sprintBacklogItemId, request);

        await InitializeAndLoadAsync(sprintBacklogItemId, ct);

        Entity.ProductBacklogItemId = request.ProductBacklogItemId;
        Entity.Name = request.Name!;
        Entity.Description = request.Description!;
        Entity.Notes = request.Notes;
        Entity.EstimationPoints = request.EstimationPoints;
        Entity.Status = (SprintBacklogItemStatus)request.Status;

        return DbContext.Entry(Entity).State == EntityState.Modified;
    }
}
