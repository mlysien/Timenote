using Timenote.Domain.Entities;

namespace Timenote.Core.Services.Abstractions;

public interface IProjectService
{
    Task CreateProjectAsync(Project project);
}