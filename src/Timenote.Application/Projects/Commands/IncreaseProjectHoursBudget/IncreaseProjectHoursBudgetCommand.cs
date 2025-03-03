using Timenote.Domain.ValueObjects;
using Timenote.Shared.Messaging;

namespace Timenote.Application.Projects.Commands.IncreaseProjectHoursBudget;

public record IncreaseProjectHoursBudgetCommand(Unique ProjectId, decimal NewHoursBudget) : ICommand<Unique>;