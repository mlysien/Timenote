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
            Budget = 2000,
            Name = "Test project",
            IsActive = true
        };

        _projectRepositoryMock.Setup(repository => repository
            .AddAsync(project)).ReturnsAsync(new Project
            {
                Id = Guid.NewGuid(),
                Budget = project.Budget,
                Name = project.Name,
                IsActive = project.IsActive,
                Worklogs = project.Worklogs
            });

        // act
        var createdProject = await _projectService.CreateProjectAsync(project);
        
        // assert
        Assert.That(createdProject, Is.Not.Null);
        Assert.That(createdProject.Id, Is.Not.Empty);
        Assert.That(createdProject.Name, Is.EqualTo(project.Name));
        Assert.That(createdProject.Budget, Is.EqualTo(project.Budget));
        
        _projectRepositoryMock.Verify(repository => repository.AddAsync(project), Times.Once);
    }
}