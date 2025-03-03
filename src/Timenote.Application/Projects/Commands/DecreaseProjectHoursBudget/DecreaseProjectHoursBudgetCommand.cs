using Timenote.Domain.ValueObjects;
using Timenote.Shared.Messaging;

namespace Timenote.Application.Projects.Commands.DecreaseProjectHoursBudget;

public record DecreaseProjectHoursBudgetCommand(Unique ProjectId, decimal NewHoursBudget) : ICommand<Unique>;