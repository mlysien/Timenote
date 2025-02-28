using Moq;
using Timenote.Common.ValueObjects;
using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.WorklogTests;

[TestFixture(TestName = "Functional tests", Description = "Functional tests")]
public class FunctionalTests
{
    private Mock<IEntryRepository> _entryRepositoryMock;
    private IWorklogService _worklogService;

    [SetUp]
    public void Setup()
    {
        _entryRepositoryMock = new Mock<IEntryRepository>();
        _worklogService = new WorklogService(_entryRepositoryMock.Object);
    }

    [Test, Description("Adding a new worklog entry")]
    public void AddWorklogEntry_AddsSingleEntryToWorklog()
    {
        // arrange
        var entry = new Entry
        {
            Id = new Unique(Guid.NewGuid()),
            StartTime = new DateTime(2025, 01, 01, 08, 0, 0),
            EndTime = new DateTime(2025, 01, 01, 16, 0, 0),
        };

        // act
        _worklogService.AddWorklogEntry(entry);

        // assert
        _entryRepositoryMock.Verify(repo => repo.Add(entry), Times.Once);
    }

    [Test, Description("Updating existing entry from worklog")]
    public void UpdateWorklogEntry_UpdatesSingleEntryFromWorklog()
    {
        // arrange
        var entry = new Entry
        {
            Id = new Unique(Guid.NewGuid()),
            StartTime = new DateTime(2025, 01, 01, 08, 0, 0),
            EndTime = new DateTime(2025, 01, 01, 16, 0, 0),
        };

        _entryRepositoryMock.Setup(m => m.Get(entry.Id)).Returns(entry);

        // act
        _worklogService.UpdateWorklogEntry(entry);

        // assert
        _entryRepositoryMock.Verify(repo => repo.Update(entry), Times.Once);
    }

    [Test, Description("Removing existing entry from worklog")]
    public void RemoveWorklogEntry_RemovesSingleEntryFromWorklog()
    {
        // arrange
        var entry = new Entry
        {
            Id = new Unique(Guid.NewGuid()),
            StartTime = new DateTime(2025, 01, 01, 08, 0, 0),
            EndTime = new DateTime(2025, 01, 01, 16, 0, 0),
        };

        _entryRepositoryMock.Setup(m => m.Get(entry.Id)).Returns(entry);

        // act
        _worklogService.RemoveWorklogEntry(entry);

        // assert
        _entryRepositoryMock.Verify(repo => repo.Remove(entry), Times.Once);
    }
}