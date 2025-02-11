using Microsoft.EntityFrameworkCore;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Implementations;

namespace Timenote.Tests.WorklogTests;

[TestFixture(TestName = "Functional tests", Description = "Functional tests")]
public class FunctionalTests
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

        // act
        service.AddWorklogEntry(entry);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(service.GetEntries().First().Id, Is.Not.Empty);
            Assert.That(service.GetEntries(), Has.Count.EqualTo(1));
            Assert.That(service.GetEntries().First().StartTime, Is.EqualTo(entry.StartTime));
            Assert.That(service.GetEntries().First().EndTime, Is.EqualTo(entry.EndTime));
        });
    }
}