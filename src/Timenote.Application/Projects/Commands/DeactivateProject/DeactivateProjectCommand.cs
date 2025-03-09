using Timenote.Application.Messaging;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Projects.Commands.DeactivateProject;

public record DeactivateProjectCommand(Unique ProjectId) : ICommand<Unique>;