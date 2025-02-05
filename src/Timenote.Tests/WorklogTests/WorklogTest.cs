﻿using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;

namespace Timenote.Tests.WorklogTests;

public class WorklogTest
{
    private IWorklogService _worklogService;

    [SetUp]
    public void Setup()
    {
        _worklogService = new WorklogService();
    }
    
    [Test, Description("Single entry can be added to Worklog")]
    public void SingleEntryCanBeAddedToWorklog()
    {
        // arrange
        var projectId = Guid.Parse("419d9040-b442-42a9-9e63-bb187941934e");
        var startTime = new DateTime(2025, 01, 01, 08, 00, 00);
        var endTime = new DateTime(2025, 01, 01, 16, 00, 00);
        
        // act
        _worklogService.AddEntry(new Entry()
        {
            StartTime = startTime,
            EndTime = endTime,
            ProjectId = projectId
        });
        
        // assert
        var entriesCollection = _worklogService.GetEntries();
        
        Assert.That(entriesCollection, Has.Count.EqualTo(1));
    }
    
    [Test, Description("Multiple entries can be added to Worklog")]
    public void MultipleEntriesCanBeAddedToWorklog()
    {
        // arrange
        var projectId = Guid.Parse("419d9040-b442-42a9-9e63-bb187941934e");
        
        var entriesToAdd = new List<Entry>()
        {
            new()
            {
                StartTime = new DateTime(2025, 01, 01, 08, 00, 00),
                EndTime = new DateTime(2025, 01, 01, 16, 00, 00),
                ProjectId = projectId
            },
            new()
            {
                StartTime = new DateTime(2025, 01, 01, 09, 00, 00),
                EndTime = new DateTime(2025, 01, 01, 17, 00, 00),
                ProjectId = projectId
            },
            new()
            {
                StartTime = new DateTime(2025, 01, 01, 06, 00, 00),
                EndTime = new DateTime(2025, 01, 01, 15, 00, 00),
                ProjectId = projectId
            }
        };
        
        var expectedCount = entriesToAdd.Count;
      
        // act
        foreach (var entry in entriesToAdd)
        {
            _worklogService.AddEntry(entry);
        }
        
        // assert
        var entriesCollection = _worklogService.GetEntries();
        
        Assert.That(entriesCollection, Has.Count.EqualTo(expectedCount));
    }
    
    [Test, Description("Worklog returns correct logged time for specified day")]
    public void WorklogReturnCorrectLoggedTime()
    {
        // arrange
        var projectId = Guid.Parse("419d9040-b442-42a9-9e63-bb187941934e");
        var startTime = new DateTime(2025, 01, 01, 08, 00, 00);
        var endTime = new DateTime(2025, 01, 01, 16, 00, 00);
        var expectedLoggedTime = new TimeSpan(8, 0, 0);

        // act
        _worklogService.AddEntry(new Entry
        {
            StartTime = startTime,
            EndTime = endTime,
            ProjectId = projectId
        });
        
        // assert
        var loggedTime = _worklogService.GetLoggedTimeForDay(new DateTime(2025, 01, 01));
        
         Assert.That(loggedTime, Is.EqualTo(expectedLoggedTime));
    } 

    [Test, Description("Worklog should throw exception when entry end time is earlier than start time")]
    public void WorklogStartTimeCannotBeGreaterThanEndTime()
    {
        // arrange
        var projectId = Guid.Parse("419d9040-b442-42a9-9e63-bb187941934e");
        var startTime = new DateTime(2025, 01, 01, 15, 00, 00);
        var endTime = new DateTime(2025, 01, 01, 08, 00, 00);
        var entry = new Entry()
        {
            StartTime = startTime,
            EndTime = endTime,
            ProjectId = projectId
        };
        
        // act & assert
        Assert.Throws<InvalidWorklogEntryException>(() => _worklogService.AddEntry(entry));
    }

    [Test, Description("Worklog should throw exception when one of entry time is empty or default")]
    public void WorklogStartTimeAndEndTimeCannotBeEmpty()
    {
        // arrange
        var projectId = Guid.Parse("419d9040-b442-42a9-9e63-bb187941934e");
        var s1 = new DateTime();
        var e1 = new DateTime();
        
        var s2 = DateTime.MinValue;
        var e2 = DateTime.MaxValue;

        var entry1 = new Entry { StartTime = s1, EndTime = e1, ProjectId = projectId };
        var entry2 = new Entry { StartTime = s2, EndTime = e2, ProjectId = projectId };
        
        // act & assert
        Assert.Throws<InvalidWorklogEntryException>(() => _worklogService.AddEntry(entry1));
        Assert.Throws<InvalidWorklogEntryException>(() => _worklogService.AddEntry(entry2));
    }
    
    [Test, Description("Worklog should throw exception when entry is null")]
    public void WorklogEntryCannotBeNull()
    {
        // arrange
        Entry entry = null;
        
        // act & assert
        Assert.Throws<InvalidWorklogEntryException>(() => _worklogService.AddEntry(entry));
    }
    
    [Test, Description("Worklog entry must have ProjectId")]
    public void WorklogEntryMustHaveProjectId()
    {
        // arrange
        var startTime = new DateTime(2025, 01, 01, 08, 00, 00);
        var endTime = new DateTime(2025, 01, 01, 16, 00, 00);
        var entry = new Entry
        {
            StartTime = startTime,
            EndTime = endTime
        };
        
        // act & assert
        Assert.Throws<InvalidWorklogEntryException>(() => _worklogService.AddEntry(entry));
    }
}
