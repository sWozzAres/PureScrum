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

            var pbi4 = new ProductBacklogItem() { Id = "4" };

            var pbi5 = new ProductBacklogItem() { Id = "5" };
            pbi5.DependentOn.Add(new ProductBacklogItemShort() { Id = "6", Name = "6" });

            var pbi6 = new ProductBacklogItem() { Id = "6" };
            pbi6.DependentOn.Add(new ProductBacklogItemShort() { Id = "5", Name = "5" });

            RepeatedField<ProductBacklogItem> pbis =
            [
                pbi2,
                pbi3,
                pbi1,
                pbi4,
                pbi5,
                pbi6,
            ];

            // Act
            var (items, circularDependencies) = TopologicalSort.Sort(pbis);

            // Assert
            Assert.True(items.Count == pbis.Count, "Sort should return all items.");
            Assert.Equal(2, circularDependencies.Count);
            Assert.Equal(4, circularDependencies[0].Count);
            Assert.Equal(3, circularDependencies[1].Count);

            Assert.Equal(circularDependencies[0][0], pbi2.Id);
            Assert.Equal(circularDependencies[0][1], pbi3.Id);
            Assert.Equal(circularDependencies[0][2], pbi1.Id);
            Assert.Equal(circularDependencies[0][3], pbi2.Id);

            Assert.Equal(circularDependencies[1][0], pbi5.Id);
            Assert.Equal(circularDependencies[1][1], pbi6.Id);
            Assert.Equal(circularDependencies[1][2], pbi5.Id);
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

            RepeatedField<ProductBacklogItem> pbis =
            [
                pbi1,
                pbi4,
                pbi3,
                pbi2
            ];

            // Act
            var (items, _) = TopologicalSort.Sort(pbis);

            // Assert
            Assert.True(items.Count == pbis.Count, "Sort should return all items.");
            Assert.Equal("4", items[0].Id);
            Assert.Equal("3", items[1].Id);
            Assert.Equal("2", items[2].Id);
            Assert.Equal("1", items[3].Id);
        }
    }
}