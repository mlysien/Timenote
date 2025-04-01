using Moq;
using Shouldly;
using Timenote.Application.Common;
using Timenote.Application.Users.Commands.ChangeRole;
using Timenote.Domain.Entities;
using Timenote.Domain.Enums;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.Users.Commands;

[TestFixture]
public class ChangeRoleCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldChangeUserRole()
    {
        // arrange
        const UserRole newRole = UserRole.Employee;
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
        
        var command = new ChangeRoleCommand(user.Id, newRole);
        var handler = new ChangeRoleCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.None);
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(
            It.Is<User>(u => u.Id == user.Id && u.Role == newRole)), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Handle_ShouldFailure_WhenNewRoleIsTheSameAsOld()
    {
        // arrange
        const UserRole newRole = UserRole.Employee;
        var repositoryMock = new Mock<IUserRepository>();
        var user = new User
        {
            Id = new Unique(Guid.NewGuid()),
            Email = "test@test.com",
            Password = "password",
            Name = "John Snow",
            Role = UserRole.Employee
        };

        repositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var command = new ChangeRoleCommand(user.Id, newRole);
        var handler = new ChangeRoleCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.Conflict);
        result.Error.Message.ShouldNotBeEmpty();
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        repositoryMock.VerifyNoOtherCalls();
    }
}