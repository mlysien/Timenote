using Timenote.Shared.Messaging;

namespace Timenote.Application.Projects.Commands.UpdateProjectName;

public record UpdateProjectNameCommand(Guid ProjectId, string ProjectName) : ICommand<Guid>;