namespace Scrum.Api.Application.Commands.ProductBacklogItemUseCase;

public class UpdateProductBacklogItem(
    ScrumDbContext dbContext,
    ILogger<UpdateProductBacklogItem> logger) : UseCaseBase<ProductBacklogItem>(dbContext)
{
    public async Task<bool> UpdateAsync(Guid productBacklogItemId, UpdateProductBacklogItemRequest request, CancellationToken ct)
    {
        logger.LogInformation("Updating product backlog item {productBacklogItemid}, request {@updateproductbacklogitemrequest}.",
            productBacklogItemId, request);

        await InitializeAndLoadAsync(productBacklogItemId, ct);

        Entity.ProductId = request.ProductId!.Value;
        Entity.Name = request.Name!;
        Entity.Description = request.Description!;
        Entity.Status = (PbiStatus)request.Status;
        Entity.EstimationPoints = request.EstimationPoints;
        Entity.Notes = request.Notes;
        Entity.DeliveryDate = request.DeliveryDate;
        Entity.IsFixedDeliveryDate = request.IsFixedDeliveryDate;
        Entity.Value = request.Value;
        Entity.Roi = request.Roi;
        Entity.SprintId = request.SprintId;

        return DbContext.Entry(Entity).State == EntityState.Modified;
    }
}
