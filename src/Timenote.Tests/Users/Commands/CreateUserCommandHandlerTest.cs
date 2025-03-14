using Moq;
using Shouldly;
using Timenote.Application.Common;
using Timenote.Application.Users.Commands.CreateUser;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.Users.Commands;

[TestFixture]
public class CreateUserCommandHandlerTest
{
    [Test]
    public async Task Handle_ShouldCreateUser()
    {
        // arrange
        const string userName = "John Snow";
        const string email = "john@snow.com";
        const string password = "snow123!";
        
        var repositoryMock = new Mock<IUserRepository>();
        
        repositoryMock.Setup(r => r.ExistsAsync(It.IsAny<Unique>())).ReturnsAsync(true);

        var command = new CreateUserCommand(userName, email, password);
        var handler = new CreateUserCommandHandler(repositoryMock.Object);
    
        // act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldNotBe(new Unique(Guid.Empty));
        result.Error.ShouldBe(Error.None);
        
        repositoryMock.Verify(r => r.EmailExistsAsync(email), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        repositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(
            p => p.Name == userName && p.Email == email && p.Password == password)
        ), Times.Once);
    }
}