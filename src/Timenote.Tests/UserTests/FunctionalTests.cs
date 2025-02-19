using Moq;
using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.UserTests;

[TestFixture(TestName = "Functional tests of User scope")]
public class FunctionalTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private IUserService _userService;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }
    
    [Test, Description("Creating new User should invoke repository method for add User entity")]
    public async Task CreateUser_ShouldAddUserEntity()
    {
        // arrange
        var user = new User
        {
            Name = "John Doe",
            Email = "john.doe@timenote.com",
        };

        _userRepositoryMock.Setup(repository => repository
            .AddAsync(user)).ReturnsAsync(new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@timenote.com",
        });

        // act
        var createdUser = await _userService.CreateUserAsync(user);
        
        // assert
        Assert.That(createdUser, Is.Not.Null);
        Assert.That(createdUser.Id, Is.Not.Empty);
        Assert.That(createdUser.Name, Is.EqualTo(user.Name));
        Assert.That(createdUser.Email, Is.EqualTo(user.Email));
        
        _userRepositoryMock.Verify(repository => repository.AddAsync(user), Times.Once);
    }
    
    [Test, Description("Updating existed User should invoke repository method for modify User entity")]
    public async Task UpdateUser_WhenExists_ShouldUpdateUserEntity()
    {
        // arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@timenote.com"
        };
        
        _userRepositoryMock.Setup(r => r.ExistsAsync(user.Id)).ReturnsAsync(true);
        _userRepositoryMock.Setup(repository => repository.UpdateAsync(user)).ReturnsAsync(user);
        
        // act
        var result = await _userService.UpdateUserAsync(user);

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(user.Id));
        Assert.That(result.Name, Is.EqualTo(user.Name));
        
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }
    
    [Test, Description("Updating not existed User should throw an exception")]
    public void UpdateUser_WhenNotExists_ShouldThrowException()
    {
        // arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@timenote.com"
        };
        
        _userRepositoryMock.Setup(r => r.ExistsAsync(user.Id)).ReturnsAsync(false);
        _userRepositoryMock.Setup(repository => repository.UpdateAsync(user)).ReturnsAsync(user);
        
        // act
        var exception = Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateUserAsync(user));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.Not.Empty);
        Assert.That(exception.Message, Contains.Substring(user.Id.ToString()));
        
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Never);
    }
    
    [Test, Description("Removing existed User should remove User entity")]
    public async Task RemoveUser_WhenExists_ShouldRemoveUserEntity()
    {
        // arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@timenote.com"
        };
        
        _userRepositoryMock.Setup(r => r.ExistsAsync(user.Id)).ReturnsAsync(true);
        
        // act
        await _userService.RemoveUserAsync(user);

        // assert
        _userRepositoryMock.Verify(r => r.RemoveAsync(user.Id), Times.Once);
    }
    
    [Test, Description("Removing not existed User should throw an exception")]
    public void RemoveUser_WhenNotExists_ShouldThrowException()
    {
        // arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@timenote.com"
        };
        
        _userRepositoryMock.Setup(r => r.ExistsAsync(user.Id)).ReturnsAsync(false);
        
        // act
        var exception = Assert.ThrowsAsync<UserNotFoundException>(() => _userService.RemoveUserAsync(user));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.Not.Empty);
        Assert.That(exception.Message, Contains.Substring(user.Id.ToString()));
        
        _userRepositoryMock.Verify(r => r.RemoveAsync(user.Id), Times.Never);
    }
}