using System.ComponentModel.DataAnnotations;

namespace Scrum.Shared;

public class CreateDefinitionOfDoneRequest
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
}

public record DefinitionOfDoneFullDto(Guid Id, string Name, string Description);
public record DefinitionOfDoneListDto(Guid Id, string Name, string Description);

public class UpdateDefinitionOfDoneRequest(string name, string description)
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; } = name;
    [Required]
    public string Description { get; set; } = description;
}
