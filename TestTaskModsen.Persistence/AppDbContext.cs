using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Configurations;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RegistrationEntity> Registrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new RegistrationConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}