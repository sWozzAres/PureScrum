namespace Scrum.Api.Application.Commands.SprintUseCase;

public class CreateSprint(ScrumDbContext dbContext,
    ILogger<CreateSprint> logger) : UseCaseBase<Sprint>(dbContext)
{
    public Sprint Create(CreateSprintRequest command)
    {
        logger.LogInformation("Creating sprint, request {@createsprintrequest}.", command);

        var sprint = new Sprint(command.SprintGoal, command.Name);
        DbContext.Sprints.Add(sprint);

        //var sprintEstimation = new SprintEstimation(sprint.Id);
        //dbContext.SprintEstimations.Add(sprintEstimation);

        return sprint;
    }
}
