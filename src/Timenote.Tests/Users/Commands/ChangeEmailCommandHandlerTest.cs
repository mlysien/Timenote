using Moq;
using Shouldly;
using Timenote.Application.Common;
using Timenote.Application.Users.Commands.ChangeEmail;
using Timenote.Domain.Entities;
using Timenote.Domain.Enums;
using Timenote.Domain.Exceptions;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.Users.Commands;

[TestFixture]
public class ChangeEmailCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldChangeUserEmail()
    {
        // arrange
        const string newEmail = "john.snow@timenote.com";
        var repositoryMock = new Mock<IUserRepository>();
        var user = new User
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "test@test.com",
            Password = "password",
            Name = "John Snow",
            Role = UserRole.Unassigned
        };

        repositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        repositoryMock.Setup(r => r.EmailExistsAsync(newEmail)).ReturnsAsync(false);
        
        var command = new ChangeEmailCommand(user.Id, newEmail);
        var handler = new ChangeEmailCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.None);
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        repositoryMock.Verify(r => r.EmailExistsAsync(newEmail), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(
            It.Is<User>(u => u.Id == user.Id && u.Email == newEmail)), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Handle_ShouldFailure_WhenEmailIsTheSameAsPrevious()
    {
        // arrange
        const string newEmail = "john.snow@timenote.com";
        var repositoryMock = new Mock<IUserRepository>();
        var user = new User
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "john.snow@timenote.com"
        };

        repositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        repositoryMock.Setup(r => r.EmailExistsAsync(newEmail)).ReturnsAsync(false);
        
        var command = new ChangeEmailCommand(user.Id, newEmail);
        var handler = new ChangeEmailCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.Conflict);
        result.Error.Message.ShouldNotBeEmpty();
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        repositoryMock.Verify(r => r.EmailExistsAsync(newEmail), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        repositoryMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Handle_ShouldFailure_WhenNewEmailAlreadyExists()
    {
        // arrange
        const string newEmail = "tyrion.lannister@timenote.com";
        var repositoryMock = new Mock<IUserRepository>();
        var user = new User
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "john.snow@timenote.com"
        };

        repositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        repositoryMock.Setup(r => r.EmailExistsAsync(newEmail)).ReturnsAsync(true);
        
        var command = new ChangeEmailCommand(user.Id, newEmail);
        var handler = new ChangeEmailCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.Conflict);
        result.Error.Message.ShouldNotBeEmpty();
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Never);
        repositoryMock.Verify(r => r.EmailExistsAsync(newEmail), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        repositoryMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Handle_ShouldFailure_WhenUserNotExist()
    {
        // arrange
        const string newEmail = "john.snow@timenote.com";
        var repositoryMock = new Mock<IUserRepository>();
        var user = new User
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "john.snow@timenote.com"
        };

        repositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ThrowsAsync(new UserNotFoundException(user.Id));
        repositoryMock.Setup(r => r.EmailExistsAsync(newEmail)).ReturnsAsync(false);
        
        var command = new ChangeEmailCommand(user.Id, newEmail);
        var handler = new ChangeEmailCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.NotFound);
        result.Error.Message.ShouldNotBeEmpty();
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        repositoryMock.Verify(r => r.EmailExistsAsync(newEmail), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        repositoryMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Handle_ShouldFailure_WhenRepositoryThrowsException()
    {
        // arrange
        const string newEmail = "john.snow@timenote.com";
        var repositoryMock = new Mock<IUserRepository>();
        var user = new User
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "tom.mom@timenote.com"
        };

        repositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        repositoryMock.Setup(r => r.EmailExistsAsync(newEmail)).ReturnsAsync(false);
        repositoryMock.Setup(r => r.UpdateAsync(It.Is<User>(x => x.Email == newEmail)))
            .ThrowsAsync(new Exception());
        
        var command = new ChangeEmailCommand(user.Id, newEmail);
        var handler = new ChangeEmailCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.Failure);
        result.Error.Message.ShouldNotBeEmpty();
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        repositoryMock.Verify(r => r.EmailExistsAsync(newEmail), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
    }
}