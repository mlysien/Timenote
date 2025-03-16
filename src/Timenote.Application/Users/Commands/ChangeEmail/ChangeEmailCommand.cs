using Timenote.Application.Messaging;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Users.Commands.ChangeEmail;

/// <summary>
/// Updates <see cref="User"/> entity with new email
/// </summary>
/// <param name="UserId">User identifier</param>
/// <param name="NewEmail">New user email</param>
public record ChangeEmailCommand(Unique UserId, string NewEmail) : ICommand;