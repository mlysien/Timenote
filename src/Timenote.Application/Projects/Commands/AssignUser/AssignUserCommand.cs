using Timenote.Application.Messaging;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Projects.Commands.AssignUser;

public record AssignUserCommand(Unique ProjectId, Unique UserId) : ICommand;