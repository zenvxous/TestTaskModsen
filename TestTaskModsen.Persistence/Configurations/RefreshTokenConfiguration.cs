using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(rt => rt.Token);
        
        builder.Property(rt => rt.UserId)
            .IsRequired();
        
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(rt => rt.Expiration)
            .IsRequired();

        builder.HasOne(rt => rt.User);
    }
}