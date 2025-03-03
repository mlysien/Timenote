using Moq;
using Timenote.Application.Projects.Commands.DecreaseProjectHoursBudget;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Shared.Common;

namespace Timenote.Tests.Projects.Commands;

[TestFixture]
public class DecreaseProjectHoursBudgetCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenProjectExists()
    {
        // arrange
        const decimal newHoursBudget = 1000;
        
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2000
        };
        
        var repositoryMock = new Mock<IProjectRepository>();
        
        repositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        
        var command = new DecreaseProjectHoursBudgetCommand(project.Id, newHoursBudget);
        var handler = new DecreaseProjectHoursBudgetCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(project), Times.Once);
        repositoryMock.Verify(r=> r.UpdateAsync(It.Is<Project>(p => p.HoursBudget == newHoursBudget)), Times.Once);
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.TypeOf<Unique>());
        Assert.That((Guid)result.Value, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.None));
    }
    
    [Test]
    public async Task Handle_ShouldReturnFailure_WhenProjectNotFound()
    {
        // arrange
        const decimal newHoursBudget = 1000;
        
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2000
        };
        
        var repositoryMock = new Mock<IProjectRepository>();

        repositoryMock.Setup(r => r.GetByIdAsync(project.Id))!.ReturnsAsync(value: null);
        
        var command = new DecreaseProjectHoursBudgetCommand(project.Id, newHoursBudget);
        var handler = new DecreaseProjectHoursBudgetCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(project), Times.Never);
      
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error.Description, Is.Not.Null);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.NotFound));
    }
    
    [Test]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowException()
    {
        // arrange
        const decimal newHoursBudget = 1000;
        
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2000
        };
        
        var repositoryMock = new Mock<IProjectRepository>();

        repositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        repositoryMock.Setup(r => r.UpdateAsync(project)).ThrowsAsync(new Exception());
        
        var command = new DecreaseProjectHoursBudgetCommand(project.Id, newHoursBudget);
        var handler = new DecreaseProjectHoursBudgetCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(project), Times.Once);
      
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error.Description, Is.Not.Null);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Failure));
    }
    
    [Test]
    public async Task Handle_ShouldReturnFailure_WhenDecreasedHoursAreMoreThanCurrent()
    {
        // arrange
        const decimal newHoursBudget = 2048;
        
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2000
        };
        
        var repositoryMock = new Mock<IProjectRepository>();

        repositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        
        var command = new DecreaseProjectHoursBudgetCommand(project.Id, newHoursBudget);
        var handler = new DecreaseProjectHoursBudgetCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(project), Times.Never);
      
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error.Description, Is.Not.Null);
        Assert.That(result.Error.Description, Does.Contain("hours budget"));
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Conflict));
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenDecreasedHoursAreLessThanBurnedHours()
    {
        // arrange
        const decimal newHoursBudget = 512;
        
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2000,
            BurnedHours = 1024
        };
        
        var repositoryMock = new Mock<IProjectRepository>();

        repositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        
        var command = new DecreaseProjectHoursBudgetCommand(project.Id, newHoursBudget);
        var handler = new DecreaseProjectHoursBudgetCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(project), Times.Never);
      
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error.Description, Is.Not.Null);
        Assert.That(result.Error.Description, Does.Contain("burned hours"));
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Conflict));
    }

}