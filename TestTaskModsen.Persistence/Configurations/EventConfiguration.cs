using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Title)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(e => e.Description).HasMaxLength(500);
        
        builder.Property(e => e.StartDate).IsRequired();
        
        builder.Property(e => e.EndDate).IsRequired();
        
        builder.Property(e => e.Location).HasMaxLength(50);
        
        builder.Property(e => e.Category).IsRequired();
        
        builder.Property(e => e.Capacity).IsRequired();

        builder
            .HasMany(e => e.Registrations)
            .WithOne(e => e.Event)
            .HasForeignKey(e => e.EventId);
    }
}