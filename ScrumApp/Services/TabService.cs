namespace ScrumApp.Services;
public class TabService
{
    Dictionary<string, int> _tabs { get; set; } = new();
    public IReadOnlyDictionary<string, int> Tabs => _tabs;

    public void ResetTabs(string key) => RememberTab(key, 0);
    public void RememberTab(string key, int index)
    {
        if (_tabs.ContainsKey(key))
        {
            _tabs[key] = index;
        }
        else
        {
            _tabs.Add(key, index);
        }
    }
    public bool TryGetValue<TEnum>(string key, out TEnum index) where TEnum : notnull, Enum
    {
        if (_tabs.TryGetValue(key, out int tabPage))
        {
            index = (TEnum)Enum.ToObject(typeof(TEnum), tabPage);
            return true;
        }
        index = (TEnum)Enum.ToObject(typeof(TEnum), 0);
        return false;
    }
}