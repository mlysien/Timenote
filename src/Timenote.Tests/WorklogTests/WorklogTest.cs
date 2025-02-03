using Timenote.Domain.Entities;

namespace Timenote.Tests.WorklogTests;

public class WorklogTest
{
    [Test, Description("Entry can be added to Worklog")]
    public void EntryCanBeAddedToWorklog()
    {
        // arrange
        var startTime = new DateTime(2025, 01, 01, 08, 00, 00);
        var endTime = new DateTime(2025, 01, 01, 16, 00, 00);
        var worklog = new Worklog();
        var logEntry = new Entry()
        {
            StartTime = startTime,
            EndTime = endTime,
        };
        
        // act
        worklog.AddEntry(logEntry);
        
        // assert
        Assert.That(worklog.Entries, Has.Count.EqualTo(1));
    }
}
