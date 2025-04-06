using Moq;
using Shouldly;
using Timenote.Application.Common;
using Timenote.Application.WorkLogs.Commands.LogWorkTime;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.WorkLogs.Commands;

[TestFixture]
public class LogWorkTimeCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldLogWorkTime()
    {
        // arrange
        var userRepository = new Mock<IUserRepository>();
        var projectRepository = new Mock<IProjectRepository>();
        var workLogRepository = new Mock<IWorkLogRepository>();

        var workTimeEntry = new WorkTimeEntry
        {
            Id = new Unique(Guid.NewGuid()),
            UserId = new Unique(Guid.NewGuid()),
            ProjectId = new Unique(Guid.NewGuid()),
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1),
            Description = "Worked on project"
        };

        userRepository.Setup(r => r.ExistsAsync(workTimeEntry.UserId)).ReturnsAsync(true);
        projectRepository.Setup(r => r.ExistsAsync(workTimeEntry.ProjectId)).ReturnsAsync(true);

        var command = new LogWorkTimeCommand(workTimeEntry.UserId, workTimeEntry.ProjectId,
            workTimeEntry.StartTime, workTimeEntry.EndTime, workTimeEntry.Description);

        var handler =
            new LogWorkTimeCommandHandler(workLogRepository.Object, userRepository.Object, projectRepository.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.None);

        userRepository.Verify(r => r.ExistsAsync(workTimeEntry.UserId), Times.Once);
        projectRepository.Verify(r => r.ExistsAsync(workTimeEntry.ProjectId), Times.Once);
        workLogRepository.Verify(r => r.AddAsync(
                It.Is<WorkTimeEntry>(
                    w => w.Description == workTimeEntry.Description)), Times.Once);
        workLogRepository.VerifyNoOtherCalls();
    }
}

