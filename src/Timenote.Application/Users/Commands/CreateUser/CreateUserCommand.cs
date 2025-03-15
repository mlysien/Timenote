using Timenote.Application.Messaging;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Users.Commands.CreateUser;

/// <summary>
/// Creates new <see cref="User"/> entity.
/// </summary>
/// <param name="Name">User name</param>
/// <param name="Email">User email (unique)</param>
/// <param name="Password">User password</param>
/// <returns>Identifier of created User</returns>

public record CreateUserCommand(string Name, string Email, string Password) : ICommand<Unique>;