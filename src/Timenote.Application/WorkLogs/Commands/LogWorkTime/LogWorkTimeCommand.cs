using Timenote.Application.Messaging;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.WorkLogs.Commands.LogWorkTime;

public record LogWorkTimeCommand(Unique UserId, Unique ProjectId, 
    DateTime StartTime, DateTime EndTime, string Description) : ICommand;
