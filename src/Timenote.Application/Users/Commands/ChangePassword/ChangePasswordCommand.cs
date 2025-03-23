using Timenote.Application.Messaging;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Users.Commands.ChangePassword;

public record ChangePasswordCommand(Unique UserId, string OldPassword, string NewPassword) : ICommand;