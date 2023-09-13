namespace ScrumApp.Helpers;

public enum Icon
{
    Details, Dependency, PBI, SBI, Product, Sprint, Home,
    ScrumBoard, Dash, Plus, SortDown, Impediment, Burndown,
    DefinitionOfDone, Charts, QuickOpen, QuickClose
}

/// <summary>
/// A lookup helper for icons, enabling easy change of the icon across the 
/// entire system.
/// </summary>
public static class IconHelper
{
    public static readonly Dictionary<Icon, string> ClassName = new()
    {
        { Icon.Details, "bi-pencil-square" },
        { Icon.PBI, "bi-database-fill" },
        { Icon.SBI, "bi-database" },
        { Icon.Product, "bi-unity" },
        { Icon.Sprint, "bi-stopwatch-fill" },
        { Icon.Home, "bi-house-door-fill" },
        { Icon.Dependency, "bi-diagram-3" },
        { Icon.ScrumBoard, "bi-calendar2-week " },
        { Icon.Dash, "bi-dash-square" },
        { Icon.Plus, "bi-plus-square" },
        { Icon.SortDown, "bi-arrow-down" },
        { Icon.Impediment, "bi-sign-stop" },
        { Icon.Burndown, "bi-graph-down-arrow" },
        { Icon.DefinitionOfDone, "bi-clipboard-check" },
        { Icon.Charts, "bi-bar-chart" },
        { Icon.QuickOpen, "bi-layout-text-sidebar-reverse" },
        { Icon.QuickClose, "bi-x-lg" },
    };

    //public static string ClassNameWith(Icon icon, string additional)
    //    => string.Join(" ", ClassName[icon], additional);
}
