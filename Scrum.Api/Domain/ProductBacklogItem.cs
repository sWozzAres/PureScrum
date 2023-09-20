using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrum.Api.Domain
{
    /// <summary>
    /// 1. Represents a set of requirements.
    /// 2. Represents the Enabling Specification for some product update that the
    ///     development team will develop.
    /// 3. Describes deliverables in business (end-user and market) terms.
    /// 4. Describes something that the Development Team​ can develop and deliver to 
    ///     add value to relevant stakeholders when Done.
    /// 5. NOT - user stories, story boards, interaction diagrams, prototypes, user narratives, etc
    ///     a) One PBI may satisfy the requirements germane to several user stories.
    /// 6. Unit of administration, estimation, and delivery
    /// 7. Should focus on what, when and for whom rather than how.
    /// 8. Most PBIs relate to the Regular Product Increment, however a PBI can describe any 
    ///     deliverable that increases stakeholder value.
    /// 9. A PBI can describe anything that has potential value to a stakeholder
    ///     a) market
    ///     b) reduces cost to the enterprise
    ///     c) reduces effort for the development team
    ///     d) a tool that helps the Product Owner Team better do its work
    /// 10. can describe anything that has potential value to a stakeholder.
    /// 11. the focus of the best PBIs is on a product solution that lies in the Vision rather than on the requirements behind that contribution to the increment.
    /// 12. lives in the solution space rather than in the problem space,
    /// 13. A PBI clarifies what to build but does not specify how to build it.
    /// </summary>
    public partial class ProductBacklogItem(Guid productId, string name, string description, Guid? sprintId)
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid ProductId { get; set; } = productId;
        public Product Product { get; private set; } = null!;

        public PbiStatus Status { get; set; }

        /// <summary>
        /// Helps relevant stakeholders crisply recall what the deliverable encompasses.
        /// </summary>
        [StringLength(32)]
        public string Name { get; set; } = name;

        /// <summary>
        /// Describes something that the ​Development Team​ can develop and deliver to add value 
        /// to relevant stakeholders when Done.
        /// </summary>
        public string Description { get; set; } = description;

        public float EstimationPoints { get; set; }

        /// <summary>
        /// 1. Notes about the PBI’s contribution to Value and ROI.
        /// 2. Stakeholder-facing decisions that the Product Owner has taken.
        /// 3. Agreements between the Product Owner and Development Team—the things they 
        ///     should write down to help them together remember.
        /// 4. Development estimates provided by the Development Team that will implement 
        ///     this PBI.
        /// </summary>
        public string? Notes { get; set; }

        public DateOnly? DeliveryDate { get; set; }

        /// <summary>
        /// Specifies that the <see cref="DeliveryDate"/> depends on a particular calendar date.
        /// </summary>
        public bool IsFixedDeliveryDate { get; set; }

        /// <summary>
        /// Individual value estimate (High Value First ordering).
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Roi estimate (ROI ordered backlog).
        /// </summary>
        public int Roi { get; set; }

        /// <summary>
        /// A PBI is usually one piece of a larger Regular Product Increment,
        /// </summary>
        public Guid? SprintId { get; set; } = sprintId;
        public Sprint? Sprint { get; private set; } = null!;


        //public DateTime CreatedTimeUtc { get; set; } = DateTime.UtcNow;
        //public Guid? CreatedBy { get; set; }

        public ICollection<ProductBacklogItem> Parents { get; } = new List<ProductBacklogItem>();
        public ICollection<ProductBacklogItem> Children { get; } = new List<ProductBacklogItem>();

        public ICollection<SprintBacklogItem> SprintBacklogItems { get; } = new List<SprintBacklogItem>();
    }
    public class ProductBacklogItemDependency(Guid parentId, Guid childId)
    {
        public Guid ParentId { get; set; } = parentId;
        public ProductBacklogItem Parent { get; private set; } = null!;
        public Guid ChildId { get; set; } = childId;
        public ProductBacklogItem Child { get; private set; } = null!;
    }
}
namespace Scrum.Api.Domain.Configuration
{
    internal class ProductBacklogItemEntityTypeConfiguration : IEntityTypeConfiguration<ProductBacklogItem>
    {
        public const string UQ_ProductBacklogItems_ProductId_Name = "UQ_ProductBacklogItems_ProductId_Name";
        public const string FK_ProductBacklogItems_Products_ProductId = "FK_ProductBacklogItems_Products_ProductId";
        public const string FK_ProductBacklogItems_Sprints_SprintId = "FK_ProductBacklogItems_Sprint_SprintId";

        public void Configure(EntityTypeBuilder<ProductBacklogItem> builder)
        {
            builder.ToTable("ProductBacklogItems", t => t.IsTemporal());

            builder.HasIndex(e => new { e.ProductId, e.Name })
                .HasDatabaseName(UQ_ProductBacklogItems_ProductId_Name)
                .IsUnique();

            builder.HasOne(e => e.Sprint)
                .WithMany(e => e.BacklogItems)
                .HasForeignKey(e => e.SprintId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName(FK_ProductBacklogItems_Sprints_SprintId);

            builder.HasOne(e => e.Product)
                .WithMany(e => e.BacklogItems)
                .HasForeignKey(e => e.ProductId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName(FK_ProductBacklogItems_Products_ProductId);

            builder.HasMany(e => e.Parents)
                .WithMany(e => e.Children)
                .UsingEntity<ProductBacklogItemDependency>(
                    "ProductBacklogItemDependencies",
                    l => l.HasOne(e => e.Parent).WithMany().HasForeignKey(e => e.ParentId),
                    r => r.HasOne(e => e.Child).WithMany().HasForeignKey(e => e.ChildId),
                    j =>
                    {
                        j.ToTable("ProductBacklogItemDependencies", x => x.IsTemporal());
                        j.HasKey(e => new { e.ParentId, e.ChildId });
                    }
                );
        }
    }
}