namespace Scrum.Api.Application.Commands.SprintUseCase;

public class DeleteSprint(ScrumDbContext dbContext) : UseCaseBase<Sprint>(dbContext)
{
    public async Task<bool> DeleteAsync(Guid sprintId, CancellationToken ct)
    {
        var loaded = await InitializeAndTryLoadAsync(sprintId, ct);
        if (loaded)
            DbContext.Sprints.Remove(Entity);
        return loaded;
    }
}
