using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QueueFlow.Domain.Entities;

namespace QueueFlow.Infrastructure.Persistence.Configurations;

public class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Title).IsRequired().HasMaxLength(200);
        builder.Property(r => r.Status).HasConversion<string>();

        builder.HasOne(r => r.Assignment)
            .WithOne()
            .HasForeignKey<Assignment>(a => a.ServiceRequestId);

        builder.HasMany(r => r.StatusHistory)
            .WithOne()
            .HasForeignKey(h => h.ServiceRequestId);
    }
}
