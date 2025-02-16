using Moq;
using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
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
    
    [Test, Description("Updating a new Project entity when exist should update existing project")]
    public async Task UpdateProject_WhenExists_ShouldUpdateProject()
    {
        // Arrange
        var project = new Project
        {
            Id = Guid.NewGuid(), 
            Name = "Project name",
            Budget = 2000,
            IsActive = true
        };
        
        var projectUpdated = new Project
        {
            Id = project.Id,
            Name = "Project name updated",
            Budget = 4000,
            IsActive = true
        };

        _projectRepositoryMock.Setup(r => r.ExistsAsync(project.Id)).ReturnsAsync(true);
        _projectRepositoryMock.Setup(repository => repository.UpdateAsync(projectUpdated)).ReturnsAsync(projectUpdated);
        
        // Act
        var result = await _projectService.UpdateProjectAsync(projectUpdated);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(projectUpdated.Id));
        Assert.That(result.Name, Is.EqualTo(projectUpdated.Name));
        _projectRepositoryMock.Verify(r => r.UpdateAsync(projectUpdated), Times.Once);
    }
    
    [Test, Description("Updating Project entity when Project doesn't exist throws an exception")]
    public void UpdateProject_WhenNotExists_ShouldThrowException()
    {
        // Arrange
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Project name updated",
            Budget = 4000,
            IsActive = true
        };

        _projectRepositoryMock.Setup(r => r.ExistsAsync(project.Id)).ReturnsAsync(false);
      
        // act
        var exception = Assert.ThrowsAsync<ProjectNotFoundException>(() => _projectService.UpdateProjectAsync(project));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.Not.Empty);
        Assert.That(exception.Message, Contains.Substring(project.Id.ToString()));
        
        _projectRepositoryMock.Verify(r => r.UpdateAsync(project), Times.Never);
    }
    
    [Test, Description("Deleting Project entity should invoke delete method from repository")]
    public void DeleteProject_WhenExists_ShouldDeleteProject()
    {
        // Arrange
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Project name",
            Budget = 4000,
            IsActive = true
        };

        _projectRepositoryMock.Setup(r => r.ExistsAsync(project.Id)).ReturnsAsync(true);
      
        // act
        _projectService.DeleteProjectAsync(project);

        // Assert
        _projectRepositoryMock.Verify(r => r.DeleteAsync(project), Times.Once);
    }    
    
    [Test, Description("Deleting Project entity when doesn't exist throws an exception")]
    public void DeleteProject_WhenDoesntExists_ThrowsException()
    {
        // Arrange
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Project name",
            Budget = 4000,
            IsActive = true
        };

        _projectRepositoryMock.Setup(r => r.ExistsAsync(project.Id)).ReturnsAsync(false);
      
        // act
        var exception = Assert.ThrowsAsync<ProjectNotFoundException>(() => _projectService.DeleteProjectAsync(project));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.Not.Empty);
        Assert.That(exception.Message, Contains.Substring(project.Id.ToString()));
        
        _projectRepositoryMock.Verify(r => r.DeleteAsync(project), Times.Never);
    }
}