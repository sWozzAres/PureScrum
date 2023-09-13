using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrum.Api.Domain
{
    public enum SprintBacklogItemStatus : int
    {
        ToDo = 0,
        InProgress = 1,
        Done = 2
    }
    /// <summary>
    /// should be any larger than a single Development Team member can complete in a single work day.
    /// </summary>
    public class SprintBacklogItem(Guid productBacklogItemId, string name, string description)
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductBacklogItemId { get; set; } = productBacklogItemId;
        public ProductBacklogItem ProductBacklogItem { get; private set; } = null!;

        public SprintBacklogItemStatus Status { get; set; }

        [StringLength(32)]
        public string Name { get; set; } = name;
        /// <summary>
        /// Reminds the Development Team of the outcome of accumulated work plan discussions to date.
        /// </summary>
        public string Description { get; set; } = description;

        /// <summary>
        /// Any notes the Developer needs to retain while working on the SBI.
        /// </summary>
        public string? Notes { get; set; }

        public float EstimationPoints { get; set; }

        public ICollection<SprintBacklogItem> Parents { get; } = new List<SprintBacklogItem>();
        public ICollection<SprintBacklogItem> Children { get; } = new List<SprintBacklogItem>();

        /// <summary>
        /// Certain SBIs depend on the completion of others.
        /// </summary>
        //public ICollection<SprintBacklogItem> DependsOn { get; } = new List<SprintBacklogItem>();
    }
    public class SprintBacklogItemDependency(Guid parentId, Guid childId)
    {
        public Guid ParentId { get; set; } = parentId;
        public SprintBacklogItem Parent { get; private set; } = null!;
        public Guid ChildId { get; set; } = childId;
        public SprintBacklogItem Child { get; private set; } = null!;
    }
}
namespace Scrum.Api.Domain.Configuration
{
    internal class SprintBacklogItemEntityTypeConfiguration : IEntityTypeConfiguration<SprintBacklogItem>
    {
        public const string UQ_SprintBacklogItems_Name = "UQ_SprintBacklogItems_Name";
        public const string FK_SprintBacklogItems_ProductBacklogItems_ProductBacklogItemsId = "FK_SprintBacklogItems_ProductBacklogItems_ProductBacklogItemsId";
        public void Configure(EntityTypeBuilder<SprintBacklogItem> builder)
        {
            builder.ToTable("SprintBacklogItems", t => t.IsTemporal());

            builder.HasIndex(e => new { e.ProductBacklogItemId, e.Name })
                .HasDatabaseName(UQ_SprintBacklogItems_Name)
                .IsUnique();

            builder.HasOne(e => e.ProductBacklogItem)
                .WithMany(e => e.SprintBacklogItems)
                .HasForeignKey(e => e.ProductBacklogItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName(FK_SprintBacklogItems_ProductBacklogItems_ProductBacklogItemsId);

            builder.HasMany(e => e.Parents)
                .WithMany(e => e.Children)
                .UsingEntity<SprintBacklogItemDependency>(
                    "SprintBacklogItemDependencies",
                    l => l.HasOne(e => e.Parent).WithMany().HasForeignKey(e => e.ParentId),
                    r => r.HasOne(e => e.Child).WithMany().HasForeignKey(e => e.ChildId),
                    j =>
                    {
                        //j.ToTable("ProductBacklogItemDependencies", x => x.IsTemporal());
                        j.HasKey(e => new { e.ParentId, e.ChildId });
                    }
                );
        }
    }
}