using Google.Protobuf.Collections;

namespace ScrumApp.Extensions;

public static class GoogleExtensions
{
    public static void RemoveAll<T>(this RepeatedField<T> collection, Predicate<T> match) where T : class
    {
        foreach (var item in collection.ToList())
        {
            if (match(item))
                collection.Remove(item);
        }
    }
    public static void RemoveAll<T>(this RepeatedField<T> collection, IEnumerable<T> list) where T : class
    {
        foreach (var item in list.ToList())
        {
            collection.Remove(item);
        }
    }
}
