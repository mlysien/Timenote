using Microsoft.EntityFrameworkCore;
using Moq;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Implementations;

namespace Timenote.Tests.WorklogTests;

public class ErrorHandlingTest
{
    private DbContextOptions<DatabaseContext> _dbContextOptions;
    
    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }
    
    [Test, Description("Worklog entry EndTime cannot be earlier than StartTime")]
    public void EntryEndTimeCannotBeEarlierThanStartTime()
    {
        // arrange
        using var context = new DatabaseContext(_dbContextOptions);
        var repository = new EntryRepository(context);
        var service = new WorklogService(repository);
        var entry = new Entry
        {
            StartTime = new DateTime(2025, 01, 01, 09, 0, 0),
            EndTime = new DateTime(2025, 01, 01, 08, 0, 0),
            ProjectId = Guid.NewGuid()
        };
        
        // act & assert
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<InvalidWorklogEntryException>(() => service.AddWorklogEntry(entry));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception!.Message, Is.Not.Empty);
        });
    }
}