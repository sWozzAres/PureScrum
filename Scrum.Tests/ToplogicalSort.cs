using Google.Protobuf.Collections;
using Scrum.Shared.Helpers;
using ScrumApi;

namespace Scrum.Tests
{
    public class ToplogicalSort
    {
        [Fact]
        public void CircularDependency_Detection()
        {
            // Arrange
            var pbi1 = new ProductBacklogItem() { Id = "1" };
            pbi1.DependentOn.Add(new ProductBacklogItemShort() { Id = "2", Name = "2" });

            var pbi2 = new ProductBacklogItem() { Id = "2" };
            pbi2.DependentOn.Add(new ProductBacklogItemShort() { Id = "3", Name = "3" });

            var pbi3 = new ProductBacklogItem() { Id = "3" };
            pbi3.DependentOn.Add(new ProductBacklogItemShort() { Id = "1", Name = "1" });

            RepeatedField<ProductBacklogItem> pbis = new()
            {
                pbi1,
                pbi3,
                pbi2
            };

            // Act
            var (items, circularDependencies) = DtoHelpers.TopologicalSort(pbis);

            // Assert
            Assert.True(items.Count == pbis.Count, "Sort should return all items.");
            Assert.Single(circularDependencies);

            Assert.Equal(circularDependencies[0], pbi1.Id);
        }

        [Fact]
        public void Sort_Ordering()
        {
            // Arrange
            var pbi1 = new ProductBacklogItem() { Id = "1" };
            pbi1.DependentOn.Add(new ProductBacklogItemShort() { Id = "2", Name = "2" });

            var pbi2 = new ProductBacklogItem() { Id = "2" };
            pbi2.DependentOn.Add(new ProductBacklogItemShort() { Id = "3", Name = "3" });

            var pbi3 = new ProductBacklogItem() { Id = "3" };
            pbi3.DependentOn.Add(new ProductBacklogItemShort() { Id = "4", Name = "4" });

            var pbi4 = new ProductBacklogItem() { Id = "4" };

            RepeatedField<ProductBacklogItem> pbis = new()
            {
                pbi1,
                pbi4,
                pbi3,
                pbi2
            };

            // Act
            var (items, _) = DtoHelpers.TopologicalSort(pbis);

            // Assert
            Assert.True(items.Count == pbis.Count, "Sort should return all items.");
            Assert.Equal("4", items[0].Id);
            Assert.Equal("3", items[1].Id);
            Assert.Equal("2", items[2].Id);
            Assert.Equal("1", items[3].Id);
        }
    }
}