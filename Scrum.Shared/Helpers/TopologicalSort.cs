namespace Scrum.Shared.Helpers;

public static class TopologicalSort
{
    public static (List<ProductBacklogItemListDto> Items, List<Guid> CircularDependencies) Sort(
        List<ProductBacklogItemListDto> items)
    {
        // initially from ChatGPT
        var circularDependencies = new List<Guid>();
        var itemsDictionary = items.ToDictionary(item => item.Id);
        var sortedIds = new List<Guid>();
        var visited = new HashSet<Guid>();
        var currentlyVisiting = new HashSet<Guid>();

        void Visit(Guid itemId)
        {
            visited.Add(itemId);
            currentlyVisiting.Add(itemId);

            foreach (var child in itemsDictionary[itemId].Children)
            {
                if (!visited.Contains(child.Id))
                {
                    Visit(child.Id);
                }
                else if (currentlyVisiting.Contains(child.Id))
                {
                    circularDependencies.Add(child.Id);
                }
            }

            currentlyVisiting.Remove(itemId);
            sortedIds.Insert(0, itemId);
        }

        foreach (var item in items)
        {
            if (!visited.Contains(item.Id))
            {
                Visit(item.Id);
            }
        }

        return (sortedIds.Select(id => itemsDictionary[id]).Reverse().ToList(), circularDependencies);
    }
    //public static (List<ScrumApi.ProductBacklogItem> Items, List<string> CircularDependencies) Sort(
    //    Google.Protobuf.Collections.RepeatedField<ScrumApi.ProductBacklogItem> items)
    //{
    //    // initially from ChatGPT
    //    var circularDependencies = new List<string>();
    //    var itemsDictionary = items.ToDictionary(item => item.Id);
    //    var sortedIds = new List<string>();
    //    var visited = new HashSet<string>();
    //    var currentlyVisiting = new HashSet<string>();

    //    void Visit(string itemId)
    //    {
    //        visited.Add(itemId);
    //        currentlyVisiting.Add(itemId);

    //        foreach (var child in itemsDictionary[itemId].DependentOn)
    //        {
    //            if (!visited.Contains(child.Id))
    //            {
    //                Visit(child.Id);
    //            }
    //            else if (currentlyVisiting.Contains(child.Id))
    //            {
    //                circularDependencies.Add(child.Id);
    //            }
    //        }

    //        currentlyVisiting.Remove(itemId);
    //        sortedIds.Insert(0, itemId);
    //    }

    //    foreach (var item in items)
    //    {
    //        if (!visited.Contains(item.Id))
    //        {
    //            Visit(item.Id);
    //        }
    //    }

    //    return (sortedIds.Select(id => itemsDictionary[id]).Reverse().ToList(), circularDependencies);
    //}
    public static (List<ScrumApi.ProductBacklogItem> Items, List<List<string>> CircularDependencies) Sort(
        Google.Protobuf.Collections.RepeatedField<ScrumApi.ProductBacklogItem> items)
    {
        var circularDependencies = new List<List<string>>();
        var itemsDictionary = items.ToDictionary(item => item.Id);
        var sortedIds = new List<string>();
        var visited = new HashSet<string>();
        var currentlyVisiting = new HashSet<string>();

        void Visit(string itemId, List<string> currentChain)
        {
            visited.Add(itemId);
            currentlyVisiting.Add(itemId);
            currentChain.Add(itemId);

            foreach (var child in itemsDictionary[itemId].DependentOn)
            {
                if (!visited.Contains(child.Id))
                {
                    Visit(child.Id, currentChain);
                }
                else if (currentlyVisiting.Contains(child.Id))
                {
                    var circularChain = new List<string>(currentChain.SkipWhile(id => id != child.Id))
                    {
                        child.Id
                    };
                    circularDependencies.Add(circularChain);
                }
            }

            currentlyVisiting.Remove(itemId);
            sortedIds.Insert(0, itemId);
            currentChain.Remove(itemId);
        }

        foreach (var item in items)
        {
            if (!visited.Contains(item.Id))
            {
                Visit(item.Id, new List<string>());
            }
        }

        return (sortedIds.Select(id => itemsDictionary[id]).Reverse().ToList(), circularDependencies);
    }
}