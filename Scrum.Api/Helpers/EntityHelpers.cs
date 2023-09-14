namespace Scrum.Api.Helpers
{
    public static class EntityHelpers
    {
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