namespace Scrum.Shared.Helpers;

public static class GoogleTypeHelpers
{
    public static DateOnly? ToDateOnly(this Google.Type.Date value)
    => (value.Year == 0 || value.Month == 0 || value.Day == 0)
        ? null
        : new(value.Year, value.Month, value.Day);
    public static Google.Type.Date FromDateOnly(this DateOnly? value)
       => value is null
        ? new() { Day = 0, Month = 0, Year = 0 }
        : new() { Day = value.Value.Day, Month = value.Value.Month, Year = value.Value.Year };
}