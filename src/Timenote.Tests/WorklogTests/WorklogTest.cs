using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;

namespace Timenote.Tests.WorklogTests;

public class WorklogTest
{
    private IWorklogService _worklogService;

    [SetUp]
    public void Setup()
    {
        _worklogService = new WorklogService();
    }
    
    [Test, Description("Entry can be added to Worklog")]
    public void EntryCanBeAddedToWorklog()
    {
        // arrange
        var startTime = new DateTime(2025, 01, 01, 08, 00, 00);
        var endTime = new DateTime(2025, 01, 01, 16, 00, 00);
        
        // act
        _worklogService.AddEntry(new Entry()
        {
            StartTime = startTime,
            EndTime = endTime,
        });
        
        // assert
        var entriesCollection = _worklogService.GetEntries();
        
        Assert.That(entriesCollection, Has.Count.EqualTo(1));
    }
}
