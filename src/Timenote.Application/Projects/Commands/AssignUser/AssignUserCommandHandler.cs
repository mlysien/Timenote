using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Shared.Common;
using Timenote.Shared.Messaging;

namespace Timenote.Application.Projects.Commands.AssignUser;

internal sealed class AssignUserCommandHandler(IProjectRepository projectRepository)
    : ICommandHandler<AssignUserCommand, Unique>
{
    public Task<Result<Unique>> Handle(AssignUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}