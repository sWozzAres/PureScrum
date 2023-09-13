using System.ComponentModel.DataAnnotations;

namespace Scrum.Shared;

public class CreateImpedimentRequest
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    public int? Severity { get; set; }
    public int? Value { get; set; }
}

public record ImpedimentFullDto(Guid Id, string Name, string Description, int? Severity, int? Value);
public record ImpedimentListDto(Guid Id, string Name, string Description, int? Severity, int? Value);

public class UpdateImpedimentRequest(string name, string description, int? severity, int? value)
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; } = name;
    [Required]
    public string Description { get; set; } = description;
    public int? Severity { get; set; } = severity;
    public int? Value { get; set; } = value;
}
