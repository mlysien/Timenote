using Timenote.Shared.Messaging;

namespace Timenote.Application.Commands.CreateProject;

public record CreateProjectCommand(string Code, string Name, long HoursBudget) : ICommand<Guid>;