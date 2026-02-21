using Microsoft.EntityFrameworkCore;
using TodoAppTakeHome.Api.Entities;

namespace TodoAppTakeHome.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
        });
    }
}
