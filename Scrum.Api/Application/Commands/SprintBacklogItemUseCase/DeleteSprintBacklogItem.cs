namespace Scrum.Api.Application.Commands.SprintBacklogItemUseCase;

public class DeleteSprintBacklogItem(ScrumDbContext dbContext) : UseCaseBase<SprintBacklogItem>(dbContext)
{
    public async Task<bool> DeleteAsync(Guid sprintbacklogitemId, CancellationToken ct)
    {
        var loaded = await InitializeAndTryLoadAsync(sprintbacklogitemId, ct);
        if (loaded)
            DbContext.SprintBacklogItems.Remove(Entity);
        return loaded;
    }
}
