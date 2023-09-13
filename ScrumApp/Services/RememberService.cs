namespace ScrumApp.Services;

public class RememberService
{
    readonly Dictionary<string, object> items = new();
    public IReadOnlyDictionary<string, object> Items => items;

    public bool TryGetValue<T>(string key, out T? value)
    {
        items.TryGetValue(key, out object? val);
        if (val != null)
        {
            value = (T)val;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    public void Remove(string key) => items.Remove(key);

    public void Remember(string key, object value)
    {
        if (items.ContainsKey(key))
            items[key] = value;
        else
            items.Add(key, value);
    }
}
