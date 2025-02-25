using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Shared.Common;
using Timenote.Shared.Messaging;

namespace Timenote.Application.Commands.UpdateProjectName;

public class UpdateProjectNameCommandHandler(IProjectRepository projectRepository) 
    : ICommandHandler<UpdateProjectNameCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateProjectNameCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        
        if (project == null)
        {
            throw new Exception();
        }
        
        project.Name = request.ProjectName;
        
        await projectRepository.UpdateAsync(project);
        
        return project.Id;
    }
}