﻿using Moq;
using Timenote.Application.Common;
using Timenote.Application.Projects.Commands.DeactivateProject;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.Projects.Commands;

[TestFixture]
public class DeactivateProjectCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldDeactivateProject_WhenProjectExists()
    {
        // arrange
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2048,
            BurnedHours = 0,
            IsActive = true
        };
        
        var repositoryMock = new Mock<IProjectRepository>();
        repositoryMock.Setup(r => r.ExistsAsync(project.Id)).ReturnsAsync(true);
        repositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        
        var command = new DeactivateProjectCommand(project.Id);
        var handler = new DeactivateProjectCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.TypeOf<Unique>());
        Assert.That(result.Value, Is.EqualTo(project.Id));
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.None));
        
        repositoryMock.Verify(r => r.UpdateAsync(It.Is<Project>(
            p=>p.IsActive == false)), Times.Once);
    }
    
    [Test]
    public async Task Handle_ShouldReturnFailure_WhenProjectIsAlreadyDeactivated()
    {
        // arrange
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2048,
            BurnedHours = 0,
            IsActive = false
        };
        
        var repositoryMock = new Mock<IProjectRepository>();
        repositoryMock.Setup(r => r.ExistsAsync(project.Id)).ReturnsAsync(true);
        repositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        
        var command = new DeactivateProjectCommand(project.Id);
        var handler = new DeactivateProjectCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error.Message, Is.Not.Empty);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Conflict));
   
        repositoryMock.Verify(r => r.UpdateAsync(It.Is<Project>(
            p=>p.IsActive == false)), Times.Never);
    }
    
    [Test]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {    
        // arrange
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2048,
            IsActive = true
        };
        
        var projectRepository = new Mock<IProjectRepository>();
        
        projectRepository.Setup(r => r.ExistsAsync(project.Id)).ReturnsAsync(true);
        projectRepository.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        projectRepository.Setup(r => r.UpdateAsync(project)).ThrowsAsync(It.IsAny<Exception>());
        
        
        var command = new DeactivateProjectCommand(project.Id);
        var handler = new DeactivateProjectCommandHandler(projectRepository.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Failure));
        Assert.That(result.Error.Message, Is.Not.Empty);
        
        projectRepository.Verify(r 
            => r.UpdateAsync(It.Is<Project>(p => p.IsActive == false)), Times.Once);
    }
}