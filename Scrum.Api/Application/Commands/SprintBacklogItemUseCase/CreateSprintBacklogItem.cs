namespace Scrum.Api.Application.Commands.SprintBacklogItemUseCase;

public class CreateSprintBacklogItem(ScrumDbContext dbContext,
    ILogger<CreateSprintBacklogItem> logger) : UseCaseBase<SprintBacklogItem>(dbContext)
{
    public SprintBacklogItem Create(CreateSprintBacklogItemRequest command)
    {
        logger.LogInformation("Creating sprint backlog item, request {@createsprintbacklogitemrequest}.", command);

        var sprintBacklogItem = new SprintBacklogItem(command.ProductBacklogItemId!.Value,
            command.Name, command.Description);
        DbContext.SprintBacklogItems.Add(sprintBacklogItem);

        return sprintBacklogItem;
    }
}
