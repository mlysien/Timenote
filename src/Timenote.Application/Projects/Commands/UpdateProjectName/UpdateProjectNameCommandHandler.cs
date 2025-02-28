using Timenote.Common.ValueObjects;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Shared.Common;
using Timenote.Shared.Messaging;

namespace Timenote.Application.Projects.Commands.UpdateProjectName;

public class UpdateProjectNameCommandHandler(IProjectRepository projectRepository) 
    : ICommandHandler<UpdateProjectNameCommand, Unique>
{
    public async Task<Result<Unique>> Handle(UpdateProjectNameCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var project = await projectRepository.GetByIdAsync(request.ProjectId);

            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            project.Name = request.ProjectName;

            await projectRepository.UpdateAsync(project);
            
            return project.Id;
        }
        catch (ProjectNotFoundException e)
        {
            return Result.Failure<Unique>(new Error("Project.NotFound", e.Message, ErrorType.NotFound));
        }

    }
}