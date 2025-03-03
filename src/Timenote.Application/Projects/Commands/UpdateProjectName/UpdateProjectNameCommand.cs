using Timenote.Domain.ValueObjects;
using Timenote.Shared.Messaging;

namespace Timenote.Application.Projects.Commands.UpdateProjectName;

public record UpdateProjectNameCommand(Unique ProjectId, string ProjectName) : ICommand<Unique>;