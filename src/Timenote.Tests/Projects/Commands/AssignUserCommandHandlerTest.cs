using Moq;
using Shouldly;
using Timenote.Application.Common;
using Timenote.Application.Projects.Commands.AssignUser;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.Projects.Commands;

[TestFixture]
public class AssignUserCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldAssignUserToProject_WhenUserAndProjectExists()
    {
        var user = new User()
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "user@email.com",
            Name = "user"
        };
        
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2048
        };
        
        // arrange
        var projectRepository = new Mock<IProjectRepository>();
        var userRepository = new Mock<IUserRepository>();
        
        userRepository.Setup(r => r.ExistsAsync(user.Id)).ReturnsAsync(true);
        userRepository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        
        projectRepository.Setup(r => r.ProjectExistsAsync(project.Id)).ReturnsAsync(true);
        projectRepository.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        
        var command = new AssignUserCommand(project.Id, user.Id);
        var handler = new AssignUserCommandHandler(projectRepository.Object, userRepository.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Error.ShouldBe(Error.None);
        
        projectRepository.Verify(r 
            => r.UpdateAsync(It.Is<Project>(p => p.User == user)), Times.Once);
    }
    
    [Test]
    public async Task Handle_ShouldFailure_WhenUserAlreadyAssigned()
    {
        var user = new User()
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "user@email.com",
            Name = "user"
        };
        
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2048,
            User = user
        };
        
        // arrange
        var projectRepository = new Mock<IProjectRepository>();
        var userRepository = new Mock<IUserRepository>();
        
        userRepository.Setup(r => r.ExistsAsync(user.Id)).ReturnsAsync(true);
        userRepository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        
        projectRepository.Setup(r => r.ProjectExistsAsync(project.Id)).ReturnsAsync(true);
        projectRepository.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        
        var command = new AssignUserCommand(project.Id, user.Id);
        var handler = new AssignUserCommandHandler(projectRepository.Object, userRepository.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.Conflict);
        
        projectRepository.Verify(r 
            => r.UpdateAsync(It.Is<Project>(p => p.User == user)), Times.Never);
    }
    
    [Test]
    public async Task Handle_ShouldFailure_WhenRepositoryThrowsException()
    {    
        // arrange
        var user = new User()
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "user@email.com",
            Name = "user"
        };
        
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Test Project",
            Code = "PROJECT.2025",
            HoursBudget = 2048
        };
        
        var projectRepository = new Mock<IProjectRepository>();
        var userRepository = new Mock<IUserRepository>();
        
        userRepository.Setup(r => r.ExistsAsync(user.Id)).ReturnsAsync(true);
        userRepository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        
        projectRepository.Setup(r => r.ProjectExistsAsync(project.Id)).ReturnsAsync(true);
        projectRepository.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
        projectRepository.Setup(r => r.UpdateAsync(project)).ThrowsAsync(It.IsAny<Exception>());
        
        
        var command = new AssignUserCommand(project.Id, user.Id);
        var handler = new AssignUserCommandHandler(projectRepository.Object, userRepository.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        result.ShouldNotBeNull();
        result.IsFailure.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.Failure);
        result.Error.Message.ShouldNotBeEmpty();
        
        projectRepository.Verify(r 
            => r.UpdateAsync(It.Is<Project>(p => p.User == user)), Times.Once);
    }
}