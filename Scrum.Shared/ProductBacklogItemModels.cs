using System.ComponentModel.DataAnnotations;

namespace Scrum.Shared;

public enum ProductBacklogItemStatusDto : int
{
    None = 0,
    Ready = 1,
    Done = 2
}
public class CreateProductBacklogItemRequest
{
    public Guid? SprintId { get; set; }

    [Required(ErrorMessage = "The Product field is required.")]
    public Guid? ProductId { get; set; }

    [Required]
    [StringLength(32)]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}

public class ProductBacklogItemDependentDto(Guid id, string name, ProductBacklogItemStatusDto status)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public ProductBacklogItemStatusDto Status { get; set; } = status;
}

public record ProductBacklogItemFullDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    ProductBacklogItemStatusDto Status,
    string Name,
    string Description,
    float EstimationPoints,
    string? Notes,
    DateOnly? DeliveryDate,
    bool IsFixedDeliveryDate,
    int Value,
    int Roi,
    Guid? SprintId,
    string? SprintName,
    List<ProductBacklogItemDependentDto> Children,
    List<SprintBacklogItemShortDto> SprintBacklogItems)
{
    public ProductBacklogItemListDto ToListDto()
        => new(Id, Name, Description, Status,
            ProductName, DeliveryDate, SprintId, SprintName, Value, Roi, EstimationPoints,
            Children, SprintBacklogItems, false);
}

public class ProductBacklogItemListDto(Guid id, string name, string description,
    ProductBacklogItemStatusDto status, string productName, DateOnly? deliveryDate, Guid? sprintId,
    string? sprintName, int? value, int? roi, float? estimationPoints,
    List<ProductBacklogItemDependentDto> children,
    List<SprintBacklogItemShortDto> sprintBacklogItems,
    bool missing = false)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public ProductBacklogItemStatusDto Status { get; set; } = status;
    public string ProductName { get; set; } = productName;
    public DateOnly? DeliveryDate { get; set; } = deliveryDate;
    public Guid? SprintId { get; set; } = sprintId;
    public string? SprintName { get; set; } = sprintName;
    public int? Value { get; set; } = value;
    public int? Roi { get; set; } = roi;
    public float? EstimationPoints { get; set; } = estimationPoints;
    public List<ProductBacklogItemDependentDto> Children { get; set; } = children;
    public List<SprintBacklogItemShortDto> SprintBacklogItems { get; set; } = sprintBacklogItems;
    public bool Missing { get; set; } = missing;
}

//public class UpdateProductBacklogItemRequest(Guid productId, BacklogItemStatusDto status, string name,
//    string description, int? estimatedDays, string? notes, DateOnly? deliveryDate,
//    bool isFixedDeliveryDate, int? value, int? roi)
//{
//    [Required]
//    public Guid? ProductId { get; set; } = productId;

//    [Required]
//    [StringLength(32)]
//    public string? Name { get; set; } = name;

//    public BacklogItemStatusDto Status { get; set; } = status;

//    [Required]
//    public string? Description { get; set; } = description;

//    public int? EstimatedDays { get; set; } = estimatedDays;

//    public string? Notes { get; set; } = notes;

//    public DateOnly? DeliveryDate { get; set; } = deliveryDate;

//    public bool IsFixedDeliveryDate { get; set; } = isFixedDeliveryDate;

//    public int? Value { get; set; } = value;

//    public int? Roi { get; set; } = roi;
//}
public class UpdateProductBacklogItemRequest
{
    [Required]
    public Guid? ProductId { get; set; }

    public Guid? SprintId { get; set; }

    [Required]
    [StringLength(32)]
    public string? Name { get; set; }

    public ProductBacklogItemStatusDto Status { get; set; }

    [Required]
    public string? Description { get; set; }

    public float EstimationPoints { get; set; }

    public string? Notes { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public bool IsFixedDeliveryDate { get; set; }

    public int Value { get; set; }

    public int Roi { get; set; }
    public UpdateProductBacklogItemRequest()
    {
    }
    public UpdateProductBacklogItemRequest(Guid productId, ProductBacklogItemStatusDto status, string name,
        string description, float estimationPoints, string? notes, DateOnly? deliveryDate,
        bool isFixedDeliveryDate, int value, int roi, Guid? sprintId)
    {
        ProductId = productId;
        Status = status;
        Name = name;
        Description = description;
        EstimationPoints = estimationPoints;
        Notes = notes;
        DeliveryDate = deliveryDate;
        IsFixedDeliveryDate = isFixedDeliveryDate;
        Value = value;
        Roi = roi;
        SprintId = sprintId;
    }
}

//public class AddDependencyRequest
//{
//    [Required]
//    public Guid? ProductBacklogItemId { get; set; }
//}
public class RemoveDependencyRequest
{
    [Required]
    public Guid? ProductBacklogItemId { get; set; }
}
