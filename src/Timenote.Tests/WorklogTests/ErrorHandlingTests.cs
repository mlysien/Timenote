using Moq;
using Timenote.Common.ValueObjects;
using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.WorklogTests;

[TestFixture(TestName = "Error handling tests", Description = "Contains tests for all error cases")]
public class ErrorHandlingTests
{
    private Mock<IEntryRepository> _entryRepositoryMock;
    private IWorklogService _worklogService;
    
    [SetUp]
    public void Setup()
    {
        _entryRepositoryMock = new Mock<IEntryRepository>();
        _worklogService = new WorklogService(_entryRepositoryMock.Object);
    }
    
    [Test, Description("Worklog entry EndTime cannot be earlier than StartTime")]
    public void EntryEndTimeCannotBeEarlierThanStartTime()
    {
        // arrange
        var entry = new Entry
        {
            StartTime = new DateTime(2025, 01, 01, 09, 0, 0),
            EndTime = new DateTime(2025, 01, 01, 08, 0, 0),
        };
        
        // act & assert
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<InvalidWorklogEntryException>(() => _worklogService.AddWorklogEntry(entry));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception!.Message, Is.Not.Empty);
        });
    }
    
    [Test, Description("Worklog entry cannot overlap on an existing entry")]
    public void EntryCannotOverlapOnExistingEntry()
    {
        // arrange
        _entryRepositoryMock.Setup(repository => repository.GetAll()).Returns(new List<Entry>
        {
            new()
            {
                Id = new Unique(Guid.NewGuid()),
                StartTime = new DateTime(2025, 01, 01, 08, 0, 0),
                EndTime = new DateTime(2025, 01, 01, 10, 0, 0),
            },
            new()
            {
                Id = new Unique(Guid.NewGuid()),
                StartTime = new DateTime(2025, 01, 01, 10, 0, 0),
                EndTime = new DateTime(2025, 01, 01, 12, 0, 0),
            }
        });

        var entry = new Entry()
        {
            Id = new Unique(Guid.NewGuid()),
            StartTime = new DateTime(2025, 01, 01, 09, 0, 0),
            EndTime = new DateTime(2025, 01, 01, 12, 0, 0),
        };
        
        // act
        var exception = Assert.Throws<InvalidWorklogEntryException>(() => _worklogService.AddWorklogEntry(entry));
            
        // assert
        Assert.Multiple(() =>
        {
            _entryRepositoryMock.Verify(repository => repository.Add(entry), Times.Never);
            
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
        });
    }
    
}