using Timenote.Application.Messaging;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(string Code, string Name, long HoursBudget) : ICommand<Unique>;