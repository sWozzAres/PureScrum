namespace Scrum.Api.Helpers
{
    public static class EntityHelpers
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

            void DFS(Guid itemId)
            {
                visited.Add(itemId);
                currentlyVisiting.Add(itemId);

                foreach (var child in itemsDictionary[itemId].Children)
                {
                    if (!visited.Contains(child.Id))
                    {
                        DFS(child.Id);
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
                    DFS(item.Id);
                }
            }

            return (sortedIds.Select(id => itemsDictionary[id]).Reverse().ToList(), circularDependencies);
        }

        /// <summary>
        /// Takes a list of T and loads any missing children throughout the hierachy into the list.
        /// </summary>
        /// <returns>A list of children that were missing.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        //public static async Task<List<Guid>> GetAll<T>(List<T> list,
        //    Func<HashSet<Guid>, CancellationToken, Task<List<T>>> loadMissing,
        //    CancellationToken cancellationToken = default) where T : IParent<IDependent>
        //{
        //    int recursion = 0;
        //    var allMissing = new List<Guid>();

        //    while (true)
        //    {
        //        recursion++;
        //        if (recursion > 50)
        //        {
        //            //TODO this should never happen
        //            throw new InvalidOperationException("Recursion limit exceeded.");
        //        }

        //        var missing = GetMissing(list);

        //        if (missing.Count == 0)
        //            break;

        //        allMissing.AddRange(missing);

        //        var load = await loadMissing(missing, cancellationToken);
        //        if (load.Count != missing.Count)
        //        {
        //            throw new InvalidOperationException("Failed to load all missing objects.");
        //        }
        //        list.AddRange(load);
        //    }

        //    return allMissing;
        //}

        //private static HashSet<Guid> GetMissing<T>(List<T> entities) where T : IParent<IDependent>
        //{
        //    var missing = new HashSet<Guid>();
        //    var topLevelIds = entities.Select(e => e.Id);

        //    foreach (var entity in entities)
        //    {
        //        foreach (var child in entity.Children)
        //        {
        //            if (!topLevelIds.Contains(child.Id))
        //            {
        //                missing.Add(child.Id);
        //            }
        //        }
        //    }

        //    return missing;
        //}
        public static async Task<List<Guid>> GetAll(List<ProductBacklogItem> list,
            Func<HashSet<Guid>, CancellationToken, Task<List<ProductBacklogItem>>> loadMissing,
            CancellationToken cancellationToken = default)
        {
            int recursion = 0;
            var allMissing = new List<Guid>();

            while (true)
            {
                recursion++;
                if (recursion > 50)
                {
                    //TODO this should never happen
                    throw new InvalidOperationException("Recursion limit exceeded.");
                }

                var missing = GetMissing(list);

                if (missing.Count == 0)
                    break;

                allMissing.AddRange(missing);

                var load = await loadMissing(missing, cancellationToken);
                if (load.Count != missing.Count)
                {
                    throw new InvalidOperationException("Failed to load all missing objects.");
                }
                list.AddRange(load);
            }

            return allMissing;
        }

        private static HashSet<Guid> GetMissing(List<ProductBacklogItem> entities)
        {
            var missing = new HashSet<Guid>();
            var topLevelIds = entities.Select(e => e.Id);

            foreach (var entity in entities)
            {
                foreach (var child in entity.Children)
                {
                    if (!topLevelIds.Contains(child.Id))
                    {
                        missing.Add(child.Id);
                    }
                }
            }

            return missing;
        }
    }
}