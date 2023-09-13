using System.ComponentModel.DataAnnotations;

namespace Scrum.Shared;

public enum SprintStatusDto : int
{
    None = 0,
    Ready = 1,
    Done = 2
}
public class CreateSprintRequest
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "The Sprint Goal field is required.")]
    public string SprintGoal { get; set; } = null!;
}

public record SprintFullDto(Guid Id, string SprintGoal, DateTimeOffset? ExpectedDeliveryDate, SprintStatusDto Status,
    string Name, DateTimeOffset? Started);

public record SprintListDto(Guid Id, string SprintGoal, DateTimeOffset? ExpectedDeliveryDate, SprintStatusDto Status,
    string Name);

public record SprintShortDto(Guid Id, string Name);
public class UpdateSprintRequest(string sprintGoal, DateTimeOffset? expectedDeliveryDate,
    SprintStatusDto status, string name, DateTimeOffset? started)
{
    [Required]
    public string SprintGoal { get; set; } = sprintGoal;

    [Required]
    [StringLength(32)]
    public string Name { get; set; } = name;

    public DateTimeOffset? ExpectedDeliveryDate { get; set; } = expectedDeliveryDate;
    public DateTimeOffset? Started { get; set; } = started;

    public SprintStatusDto Status { get; set; } = status;
}
/// <summary>
/// Data for Burndown chart.
/// </summary>
/// <param name="InstantUtc">The instant in time</param>
/// <param name="N">None Points</param>
/// <param name="R">Ready Points</param>
/// <param name="D">Done Points</param>
public record BurndownEntry(DateTime InstantUtc,
    float? N,
    float? R,
    float? D);

