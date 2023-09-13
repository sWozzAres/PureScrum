using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrum.Api.Domain
{
    public class DefinitionOfDone(string name, string description)
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(32)]
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
    }
}

namespace Scrum.Api.Domain.Configuration
{
    internal class DefinitionOfDoneEntityTypeConfiguration : IEntityTypeConfiguration<DefinitionOfDone>
    {
        public void Configure(EntityTypeBuilder<DefinitionOfDone> builder)
        {
            builder.ToTable("DefinitionOfDones", t => t.IsTemporal());
        }
    }
}