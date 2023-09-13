using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrum.Api.Domain
{
    public class Impediment(string name, string description, int? severity, int? value)
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(32)]
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public int? Severity { get; set; } = severity;
        public int? Value { get; set; } = value;
    }
}

namespace Scrum.Api.Domain.Configuration
{
    internal class ImpedimentEntityTypeConfiguration : IEntityTypeConfiguration<Impediment>
    {
        public void Configure(EntityTypeBuilder<Impediment> builder)
        {
            builder.ToTable("Impediments", t => t.IsTemporal());
        }
    }
}