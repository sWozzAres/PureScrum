using System.ComponentModel.DataAnnotations;

namespace Scrum.Shared;

public enum SprintBacklogItemStatusDto : int
{
    ToDo = 0,
    InProgress = 1,
    Done = 2
}
public class CreateSprintBacklogItemRequest
{
    [Required]
    public Guid? ProductBacklogItemId { get; set; }

    [Required]
    [StringLength(32)]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}

public record SprintBacklogItemShortDto(Guid Id, string Name, string Description, float? EstimationPoints, SprintBacklogItemStatusDto Status);
public record SprintBacklogItemFullDto(
    Guid Id,
    Guid ProductBacklogItemId,
    string ProductBacklogItemName,
    //Guid ProductId,
    //string ProductName,
    string Name,
    string Description,
    string? Notes,
    Guid? SprintId,
    string? SprintName,
    float EstimationPoints,
    SprintBacklogItemStatusDto Status);

public record SprintBacklogItemListDto(Guid Id, string Name,
    string ProductBacklogItemName,
    float? EstimationPoints,
    SprintBacklogItemStatusDto Status,
    string? SprintName);

public class UpdateSprintBacklogItemRequest
{
    public Guid ProductBacklogItemId { get; set; }

    public SprintBacklogItemStatusDto Status { get; set; }

    [Required]
    [StringLength(32)]
    public string? Name { get; set; }

    [Required]
    public string? Description { get; set; }

    public string? Notes { get; set; }

    public float EstimationPoints { get; set; }

    public UpdateSprintBacklogItemRequest()
    {
    }
    public UpdateSprintBacklogItemRequest(Guid productBacklogItemId, SprintBacklogItemStatusDto status,
        string name, string description, string? notes, float estimationPoints)
    {
        ProductBacklogItemId = productBacklogItemId;
        Status = status;
        Name = name;
        Description = description;
        Notes = notes;
        EstimationPoints = estimationPoints;
    }
}
public record MoveSbi(Guid PbiId, SprintBacklogItemStatusDto Status);
