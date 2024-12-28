using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .HasMany(n => n.Registrations)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);
    }
}