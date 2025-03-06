using Moq;
using Timenote.Application.Common;
using Timenote.Application.Projects.Commands.UpdateProjectName;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
using Timenote.Domain.ValueObjects;
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
            Id = new Unique(Guid.NewGuid()),
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
        Assert.That(result.Value, Is.TypeOf<Unique>());
        Assert.That((Guid)result.Value, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Value, Is.EqualTo(project.Id));
    }
    
    [Test]
    public async Task Handle_ShouldFailure_WhenProjectNotExists()
    {
        // arrange
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Code = "PROJECT.CODE",
            Name = "Test Project",
            HoursBudget = 2400
        };
        var repositoryMock = new Mock<IProjectRepository>();
        
        repositoryMock.Setup(r => r.GetByIdAsync(project.Id)).ThrowsAsync(new ProjectNotFoundException(project.Id));
        
        var command = new UpdateProjectNameCommand(project.Id, "Updated Project");
        var handler = new UpdateProjectNameCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.GetByIdAsync(project.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(project), Times.Never);
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.NotFound));
    }
    
    [Test]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {    
        // arrange
        var repositoryMock = new Mock<IProjectRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Project());
        repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Project>())).ThrowsAsync(It.IsAny<Exception>());
        
        var command = new UpdateProjectNameCommand(new Unique(Guid.NewGuid()), "New Project");
        var handler = new UpdateProjectNameCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Project>()), Times.Once);
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Failure));
    }
}