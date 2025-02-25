using Moq;
using Timenote.Application.Commands.CreateProject;
using Timenote.Domain.Entities;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Shared.Common;

namespace Timenote.Tests.Projects.Commands;

[TestFixture]
public class CreateProjectCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldCallAddOnRepository_WhenProjectNotExists()
    {
        // arrange
        var repositoryMock = new Mock<IProjectRepository>();
        repositoryMock.Setup(r => r.CodeExistsAsync("PROJECT.2025")).ReturnsAsync(false);
        var command = new CreateProjectCommand("PROJECT.2025", "New Project", 2400);
        var handler = new CreateProjectCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        repositoryMock.Verify(r => r.CodeExistsAsync("PROJECT.2025"), Times.Once);
        
        repositoryMock.Verify(repo
            => repo.AddAsync(It.Is<Project>(p =>
                p.Name == "New Project" &&
                p.Code == "PROJECT.2025" &&
                p.HoursBudget == 2400)));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Empty);
        Assert.That(result.Value, Is.TypeOf<Guid>());
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.None));
    }
    
    [Test]
    public async Task Handle_ShouldFailure_WhenProjectAlreadyExists()
    {
        // arrange
        var repositoryMock = new Mock<IProjectRepository>();
        repositoryMock.Setup(r => r.CodeExistsAsync("PROJECT.2025")).ReturnsAsync(true);
        var command = new CreateProjectCommand("PROJECT.2025", "New Project", 2400);
        var handler = new CreateProjectCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Never);
        repositoryMock.Verify(r => r.CodeExistsAsync("PROJECT.2025"), Times.Once);
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Conflict));
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {    // arrange
        var repositoryMock = new Mock<IProjectRepository>();
        repositoryMock.Setup(r => r.CodeExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        repositoryMock.Setup(r => r.AddAsync(It.IsAny<Project>())).ThrowsAsync(It.IsAny<Exception>());
        
        var command = new CreateProjectCommand("PROJECT.2025", "New Project", 2400);
        var handler = new CreateProjectCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(r => r.CodeExistsAsync("PROJECT.2025"), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Failure));
    }
}