using Moq;
using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Tests.ProjectTests;

[TestFixture(TestName = "Functional tests", Description = "Functional tests for Project entities")]
public class FunctionalTests
{
    private Mock<IProjectRepository> _projectRepositoryMock;
    private IProjectService _projectService;
    
    [SetUp]
    public void Setup()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _projectService = new ProjectService(_projectRepositoryMock.Object);
    }
    
    [Test, Description("Adding a new Project entity to database")]
    public async Task AddNewProjectShouldSaveInDatabase()
    {
        // arrange
        var project = new Project
        {
            Id = Guid.NewGuid(),
        };
        
        // act
        await _projectService.CreateProjectAsync(project);
        
        // assert
        _projectRepositoryMock.Verify(repository => repository.Add(project), Times.Once);
    }

}