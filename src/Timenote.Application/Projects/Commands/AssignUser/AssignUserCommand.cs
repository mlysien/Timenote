using Timenote.Domain.ValueObjects;

using Timenote.Shared.Messaging;

namespace Timenote.Application.Projects.Commands.AssignUser;

public record AssignUserCommand(Unique ProjectId, Unique UserId) : ICommand;