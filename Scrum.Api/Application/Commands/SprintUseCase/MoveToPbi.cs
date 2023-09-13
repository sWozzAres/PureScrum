namespace Scrum.Api.Application.Commands.SprintUseCase;

public class MoveToPbi(ScrumDbContext dbContext) : UseCaseBase<SprintBacklogItem>(dbContext)
{
    public async Task MoveAsync(Guid sprintId, MoveSbi request, CancellationToken ct)
    {
        await InitializeAndLoadAsync(sprintId, ct);
        Entity.ProductBacklogItemId = request.PbiId;
        Entity.Status = (SprintBacklogItemStatus)request.Status;
    }
}
