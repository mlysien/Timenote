using Microsoft.EntityFrameworkCore;
using Moq;
using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Persistence.Repositories.Implementations;

namespace Timenote.Tests.WorklogTests;

[TestFixture(TestName = "Happy path flow tests", Description = "Contains tests for all happy paths")]
public class HappyPathTests
{
    private Mock<IEntryRepository> _entryRepositoryMock;
    private IWorklogService _worklogService;
    
    [SetUp]
    public void Setup()
    {
        _entryRepositoryMock = new Mock<IEntryRepository>();
        _worklogService = new WorklogService(_entryRepositoryMock.Object);
    }
    
    [Test, Description("Getting entries from a day returns correct worktime")]
    public void GetEntriesFromDay_ReturnsCorrectWorktime()
    {
        // arrange
        var expectedLoggedTime = TimeSpan.FromHours(12);
        _entryRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Entry>
        {
            new()
            {
                Id = Guid.NewGuid(),
                StartTime = new DateTime(2025, 01, 01, 08, 0, 0),
                EndTime = new DateTime(2025, 01, 01, 16, 0, 0),
            },
            new()
            {
                Id = Guid.NewGuid(),
                StartTime = new DateTime(2025, 01, 01, 16, 0, 0),
                EndTime = new DateTime(2025, 01, 01, 20, 0, 0),
            },
            // this entry should not be calculated
            new() 
            {
                Id = Guid.NewGuid(),
                StartTime = new DateTime(2025, 01, 2, 8, 0, 0),
                EndTime = new DateTime(2025, 01, 2, 10, 0, 0),
            }
        });

        // act
        var loggedTime = _worklogService.GetLoggedTimeFromDay(new DateTime(2025, 01, 01));
        
        // arrange
        Assert.That(loggedTime, Is.EqualTo(expectedLoggedTime));
    } 
}