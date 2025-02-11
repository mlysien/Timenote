using Microsoft.EntityFrameworkCore;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Implementations;

namespace Timenote.Tests.WorklogTests;

[TestFixture(TestName = "Happy path flow tests", Description = "Contains tests for all happy paths")]
public class HappyPathTests
{
    private DbContextOptions<DatabaseContext> _dbContextOptions;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
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
        
        var correctDayLogs = service.GetLoggedTimeFromDay(new DateTime(2025, 01, 01));
        var incorrectDayLogs = service.GetLoggedTimeFromDay(new DateTime(2025, 12, 01));
        
        // assert
        Assert.Multiple(() =>
        {
            Assert.That(correctDayLogs, Is.EqualTo(expectedLoggedTime));
            Assert.That(incorrectDayLogs, Is.EqualTo(expectedNotLoggedTime));
        });
    } 
}