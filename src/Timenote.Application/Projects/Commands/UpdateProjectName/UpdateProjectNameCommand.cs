using Timenote.Application.Messaging;
using Timenote.Domain.ValueObjects;

namespace Timenote.Application.Projects.Commands.UpdateProjectName;

public record UpdateProjectNameCommand(Unique ProjectId, string ProjectName) : ICommand<Unique>;