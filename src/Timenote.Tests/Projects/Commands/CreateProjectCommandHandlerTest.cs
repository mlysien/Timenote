using Moq;
using Timenote.Application.Commands.CreateProject;
using Timenote.Domain.Entities;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Shared.Kernel;

namespace Timenote.Tests.Projects.Commands;

[TestFixture]
public class CreateProjectCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldCallAddOnRepository_WhenProjectNotExists()
    {
        // arrange
        var repositoryMock = new Mock<IProjectRepository>();
        var handler = new CreateProjectCommandHandler(repositoryMock.Object);
        var command = new CreateProjectCommand("New Project", 2400);

        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Once);
        
        repositoryMock.Verify(repo 
            => repo.AddAsync(It.Is<Project>(p=>p.Name == "New Project")));
        
        repositoryMock.Verify(repo 
            => repo.AddAsync(It.Is<Project>(p=>p.Budget == 2400)));
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Empty);
        Assert.That(result.Value, Is.TypeOf<Guid>());
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.None));
    }
}