using Timenote.Application.Messaging;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Projects.Commands.IncreaseProjectHoursBudget;

public record IncreaseProjectHoursBudgetCommand(Unique ProjectId, decimal NewHoursBudget) : ICommand<Unique>;