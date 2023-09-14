namespace ScrumApp.Services;

/// <summary>
/// Maintains selected tab for pages identified with a key.
/// </summary>
public class TabService
{
    readonly Dictionary<string, int> tabs = new();
    public IReadOnlyDictionary<string, int> Tabs => tabs;

    public void ResetTabs(string key) => RememberTab(key, 0);
    public void RememberTab(string key, int index)
    {
        if (!tabs.TryAdd(key, index))
        {
            tabs[key] = index;
        }
    }
    public bool TryGetValue<TEnum>(string key, out TEnum index) where TEnum : notnull, Enum
    {
        if (tabs.TryGetValue(key, out int tabPage))
        {
            index = (TEnum)Enum.ToObject(typeof(TEnum), tabPage);
            return true;
        }
        index = (TEnum)Enum.ToObject(typeof(TEnum), 0);
        return false;
    }
}