using Microsoft.EntityFrameworkCore;
using Moq;
using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Persistence.Repositories.Implementations;

namespace Timenote.Tests.WorklogTests;

public class WorklogTest
{
    private DbContextOptions<DatabaseContext> _dbContextOptions;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    
    
    [Test, Description("Updating existing entry from worklog")]
    public void UpdateWorklogEntry_UpdatesSingleEntryFromWorklog()
    {
        // arrange
        using var context = new DatabaseContext(_dbContextOptions);
        
        var repository = new EntryRepository(context);
        var service = new WorklogService(repository);
        
        var startTime = new DateTime(2025, 01, 01, 08, 0, 0);
        
        var entry = new Entry
        {
            StartTime = startTime,
            EndTime = startTime.AddHours(8),
            ProjectId = Guid.NewGuid()
        };

        // Act
        service.AddWorklogEntry(entry);

        context.ChangeTracker.Clear();
        
        Assert.That(service.GetLoggedTimeFromDay(startTime), Is.EqualTo(TimeSpan.FromHours(8)));
        
        var updatedEntry = new Entry()
        {
            Id = entry.Id,
            StartTime = startTime,
            EndTime = startTime.AddHours(10),
            ProjectId = Guid.NewGuid()
        };
        
        service.UpdateWorklogEntry(updatedEntry);
        
        // Assert
        Assert.That(service.GetLoggedTimeFromDay(startTime), Is.EqualTo(TimeSpan.FromHours(10)));
    }
    
  
    
    [Test, Description("Removing existing entry from worklog")]
    public void RemoveWorklogEntry_RemovesSingleEntryFromWorklog()
    {
        // arrange
        using var context = new DatabaseContext(_dbContextOptions);
        
        var repository = new EntryRepository(context);
        var service = new WorklogService(repository);
        
        var startTime = new DateTime(2025, 01, 01, 08, 0, 0);
        
        var entry = new Entry
        {
            StartTime = startTime,
            EndTime = startTime.AddHours(8),
            ProjectId = Guid.NewGuid()
        };

        // Act
        service.AddWorklogEntry(entry);
        
        Assert.That(service.GetEntries(), Has.Count.EqualTo(1));
        
        context.ChangeTracker.Clear();
        
        service.RemoveWorklogEntry(entry);
        
        // Assert
        Assert.That(service.GetEntries(), Has.Count.EqualTo(0));
    }
}
