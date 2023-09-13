namespace ScrumApp.Components;

public class TabInfo(string icon, string? extra = null)
{
    public string Icon { get; set; } = icon;
    public string? Extra { get; set; } = extra;
}
