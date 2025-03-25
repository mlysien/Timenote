using Moq;
using Shouldly;
using Timenote.Application.Common;
using Timenote.Application.Users.Commands.ChangePassword;
using Timenote.Domain.Entities;
using Timenote.Domain.Enums;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.Users.Commands;


[TestFixture]
public class ChangePasswordCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldChangeUserPassword()
    {
        // arrange
        const string newPassword = "newPassword!2#";
        var repositoryMock = new Mock<IUserRepository>();
        var user = new User
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "test@test.com",
            Password = "oldPassword!2#",
            Name = "John Snow",
            Role = UserRole.Unassigned
        };

        repositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var command = new ChangePasswordCommand(user.Id, user.Password, newPassword);
        var handler = new ChangePasswordCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.None);
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(
            It.Is<User>(u => u.Id == user.Id && u.Password == newPassword)), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Handle_ShouldFailure_WhenNewPasswordIsTheSameAsOld()
    {
        // arrange
        const string newPassword = "password!2#";
        var repositoryMock = new Mock<IUserRepository>();
        var user = new User
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "test@test.com",
            Password = "password!2#",
            Name = "John Snow",
            Role = UserRole.Unassigned
        };

        repositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var command = new ChangePasswordCommand(user.Id, user.Password, newPassword);
        var handler = new ChangePasswordCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.Conflict);
        result.Error.Message.ShouldNotBeEmpty();
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Never);
        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        repositoryMock.VerifyNoOtherCalls();
    }
    
}