using Moq;
using Shouldly;
using Timenote.Application.Common;
using Timenote.Application.Users.Commands.ChangeEmail;
using Timenote.Domain.Entities;
using Timenote.Domain.Enums;
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

        var command = new ChangeEmailCommand(user.Id, newEmail);
        var handler = new ChangeEmailCommandHandler(repositoryMock.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.ShouldBeTrue();
        result.Error.Type.ShouldBe(ErrorType.None);
        
        repositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(
            It.Is<User>(u => u.Id == user.Id && u.Email == newEmail)), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
    }
}