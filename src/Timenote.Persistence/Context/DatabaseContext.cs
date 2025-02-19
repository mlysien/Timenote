using Microsoft.EntityFrameworkCore;
using Timenote.Domain.Entities;

namespace Timenote.Persistence.Context;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Worklog> Worklogs { get; set; } = null!;
    public DbSet<Entry> Entries  { get; set; } = null!;

    public DbSet<Project> Projects { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Worklogs)
            .WithOne(w => w.Project)
            .HasForeignKey(w => w.ProjectId);
        
        modelBuilder.Entity<Worklog>()
            .HasKey(worklog => worklog.Id);
        
        modelBuilder.Entity<Entry>()
            .HasOne(entry => entry.Worklog)
            .WithMany(worklog => worklog.Entries)
            .HasForeignKey(entry => entry.WorklogId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Projects)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Worklogs)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId);
    }
}