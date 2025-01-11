using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Configurations;

public class RegistrationConfiguration : IEntityTypeConfiguration<RegistrationEntity>
{
    public void Configure(EntityTypeBuilder<RegistrationEntity> builder)
    {
        builder.HasKey(r => r.Id);

        builder
            .HasOne(r => r.User)
            .WithMany(u => u.Registrations);
        
        builder
            .HasOne(r => r.Event)
            .WithMany(e => e.Registrations);
    }   
}