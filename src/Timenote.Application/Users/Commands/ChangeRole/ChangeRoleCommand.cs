using Timenote.Application.Messaging;
using Timenote.Domain.Enums;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Users.Commands.ChangeRole;

public record ChangeRoleCommand(Unique UserId, UserRole NewRole) : ICommand;