namespace Scrum.Shared.Helpers;

public static class DtoHelpers
{
    public static (List<ProductBacklogItemListDto> Items, List<Guid> CircularDependencies) TopologicalSort(
        List<ProductBacklogItemListDto> items)
    {
        //// mostly from ChatGPT
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
    public static (List<ScrumApi.ProductBacklogItem> Items, List<string> CircularDependencies) TopologicalSort(
        Google.Protobuf.Collections.RepeatedField<ScrumApi.ProductBacklogItem> items)
    {
        //// mostly from ChatGPT
        var circularDependencies = new List<string>();
        var itemsDictionary = items.ToDictionary(item => item.Id);
        var sortedIds = new List<string>();
        var visited = new HashSet<string>();
        var currentlyVisiting = new HashSet<string>();

        void Visit(string itemId)
        {
            visited.Add(itemId);
            currentlyVisiting.Add(itemId);

            foreach (var child in itemsDictionary[itemId].DependentOn)
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
}