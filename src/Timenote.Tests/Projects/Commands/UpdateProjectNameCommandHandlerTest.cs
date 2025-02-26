﻿using Moq;
using Timenote.Application.Commands.UpdateProjectName;
using Timenote.Domain.Entities;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.Projects.Commands;

[TestFixture]
public class UpdateProjectNameCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldCallUpdateOnRepository_WhenProjectExists()
    {
        // arrange
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Code = "PROJECT.CODE",
            Name = "Test Project",
            HoursBudget = 2400
        };
        var repositoryMock = new Mock<IProjectRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        var command = new UpdateProjectNameCommand(project.Id, "Updated Project");
        var handler = new UpdateProjectNameCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(project), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(It.Is<Project>(p => p.Name == "Updated Project")), Times.Once);
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Empty);
        Assert.That(result.Value, Is.TypeOf<Guid>());
        Assert.That(result.Value, Is.EqualTo(project.Id));
    }

}