namespace Scrum.Api.Application.Commands.ProductBacklogItemUseCase;

public class CreateProductBacklogItem(ScrumDbContext dbContext,
    ILogger<CreateProductBacklogItem> logger) : UseCaseBase<ProductBacklogItem>(dbContext)
{
    public ProductBacklogItem Create(CreateProductBacklogItemRequest command)
    {
        logger.LogInformation("Creating product backlog item, request {@createproductbacklogitemrequest}.", command);

        var productBacklogItem = new ProductBacklogItem(command.ProductId!.Value, command.Name,
            command.Description, command.SprintId);
        DbContext.ProductBacklogItems.Add(productBacklogItem);

        // make sure there is an entry for the sprint
        //if (command.SprintId is not null)
        //{
        //    var sprintEstimation = await dbContext.SprintEstimations.FindAsync(new object[] { command.SprintId }, ct);
        //    if (sprintEstimation is null)
        //    {
        //        sprintEstimation = new SprintEstimation(command.SprintId!.Value);
        //        dbContext.SprintEstimations.Add(sprintEstimation);
        //    }
        //}
        return productBacklogItem;
    }
}
