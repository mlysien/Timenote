using Microsoft.EntityFrameworkCore;
using Timenote.Domain.Entities;

namespace Timenote.Persistence.Context;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Worklog> Worklogs { get; set; } = null!;
    public DbSet<Entry> Entries  { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Worklog>()
            .HasKey(worklog => worklog.Id);
        
        modelBuilder.Entity<Entry>()
            .HasOne(entry => entry.Worklog)
            .WithMany(worklog => worklog.Entries)
            .HasForeignKey(entry => entry.WorklogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}