using System.ComponentModel.DataAnnotations;

namespace Scrum.Shared;

public class CreateProductRequest
{
    [Required]
    [StringLength(64)]
    public string Name { get; set; } = null!;
    [Required]
    public string Vision { get; set; } = null!;
}

public record ProductFullDto(Guid Id, string Name, string Vision);
public record ProductShortDto(Guid Id, string Name);
public record ProductListDto(Guid Id, string Name, string Vision);
public record ProductListShortDto(Guid Id, string Name);

public class UpdateProductRequest(string name, string vision)
{
    [Required]
    [StringLength(64)]
    public string Name { get; set; } = name;
    [Required]
    public string Vision { get; set; } = vision;
}
