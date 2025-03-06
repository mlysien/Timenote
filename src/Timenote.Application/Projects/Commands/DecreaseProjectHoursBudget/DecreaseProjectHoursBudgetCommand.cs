using Timenote.Application.Messaging;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Projects.Commands.DecreaseProjectHoursBudget;

public record DecreaseProjectHoursBudgetCommand(Unique ProjectId, decimal NewHoursBudget) : ICommand<Unique>;