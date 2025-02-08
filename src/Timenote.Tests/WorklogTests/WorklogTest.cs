﻿using Microsoft.EntityFrameworkCore;
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

    [Test, Description("Adding a new worklog entry")]
    public void AddWorklogEntry_AddsSingleEntryToWorklog()
    {
        // arrange
        using var context = new DatabaseContext(_dbContextOptions);
        var repository = new EntryRepository(context);
        var service = new WorklogService(repository);
        var entry = new Entry
        {
            StartTime = new DateTime(2025, 01, 01, 08, 0, 0),
            EndTime = new DateTime(2025, 01, 01, 16, 0, 0),
            ProjectId = Guid.NewGuid()
        };

        // Act
        service.AddWorklogEntry(entry);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(service.GetEntries().First().Id, Is.Not.Empty);
            Assert.That(service.GetEntries(), Has.Count.EqualTo(1));
            Assert.That(service.GetEntries().First().StartTime, Is.EqualTo(entry.StartTime));
            Assert.That(service.GetEntries().First().EndTime, Is.EqualTo(entry.EndTime));
        });
    }
    
    [Test, Description("Getting entries from a day returns correct worktime")]
    public void GetEntriesFromDay_ReturnsCorrectWorktime()
    {
        // arrange
        using var context = new DatabaseContext(_dbContextOptions);
        
        var expectedLoggedTime = TimeSpan.FromHours(8);
        var expectedNotLoggedTime = TimeSpan.Zero;
        var repository = new EntryRepository(context);
        var service = new WorklogService(repository);
        var entry = new Entry
        {
            StartTime = new DateTime(2025, 01, 01, 08, 0, 0),
            EndTime = new DateTime(2025, 01, 01, 16, 0, 0),
            ProjectId = Guid.NewGuid()
        };
        
        // act
        service.AddWorklogEntry(entry);
        
        var correctDayLogs = service.GetEntriesFromDay(new DateTime(2025, 01, 01));
        var incorrectDayLogs = service.GetEntriesFromDay(new DateTime(2025, 12, 01));
        
        // assert
        Assert.Multiple(() =>
        {
            Assert.That(correctDayLogs, Is.EqualTo(expectedLoggedTime));
            Assert.That(incorrectDayLogs, Is.EqualTo(expectedNotLoggedTime));
        });
    } 
}
