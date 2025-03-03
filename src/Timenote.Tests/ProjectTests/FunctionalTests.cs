using Moq;
using Timenote.Core.Services.Abstractions;
using Timenote.Core.Services.Implementations;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
using Timenote.Domain.ValueObjects;
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

    [Test, Description("Creating a new project should call Add method on repository")]
    public async Task CreateProject_ShouldCallAddOnRepository()
    {
        // arrange
        var project = new Project
        {
            HoursBudget = 2000,
            Name = "Test project",
            IsActive = true
        };

        _projectRepositoryMock.Setup(repository => repository
            .AddAsync(project)).ReturnsAsync(new Project
            {
                Id = new Unique(Guid.NewGuid()),
                HoursBudget = project.HoursBudget,
                Name = project.Name,
                IsActive = project.IsActive,
                Worklogs = project.Worklogs
            });

        // act
        var createdProject = await _projectService.CreateProjectAsync(project);
        
        // assert
        Assert.That(createdProject, Is.Not.Null);
        Assert.That(createdProject.Id, Is.TypeOf<Unique>());
        Assert.That(createdProject.Name, Is.EqualTo(project.Name));
        Assert.That(createdProject.HoursBudget, Is.EqualTo(project.HoursBudget));
        
        _projectRepositoryMock.Verify(repository => repository.AddAsync(project), Times.Once);
    }
    
    [Test, Description("Updating Project entity should call Update on Repository when project exists")]
    public async Task UpdateProject_ShouldCallUpdateOnRepository_WhenProjectExists()
    {
        // Arrange
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()), 
            Name = "Project name",
            HoursBudget = 2000,
            IsActive = true
        };
        
        var projectUpdated = new Project
        {
            Id = project.Id,
            Name = "Project name updated",
            HoursBudget = 4000,
            IsActive = true
        };

        _projectRepositoryMock.Setup(r => r.ProjectExistsAsync(project.Id)).ReturnsAsync(true);
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
    public void UpdateProject_ShouldThrowException_WhenProjectNotExists()
    {
        // Arrange
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Project name updated",
            HoursBudget = 4000,
            IsActive = true
        };

        _projectRepositoryMock.Setup(r => r.ProjectExistsAsync(project.Id)).ReturnsAsync(false);
      
        // act
        var exception = Assert.ThrowsAsync<ProjectNotFoundException>(() => _projectService.UpdateProjectAsync(project));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.Not.Empty);
        Assert.That(exception.Message, Contains.Substring(project.Id.ToString()));
        
        _projectRepositoryMock.Verify(r => r.UpdateAsync(project), Times.Never);
    }
    
    [Test, Description("Deleting Project entity should call Delete method on repository")]
    public void DeleteProject_ShouldCallDeleteOnRepository_WhenProjectExists()
    {
        // Arrange
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Project name",
            HoursBudget = 4000,
            IsActive = true
        };

        _projectRepositoryMock.Setup(r => r.ProjectExistsAsync(project.Id)).ReturnsAsync(true);
      
        // act
        _projectService.DeleteProjectAsync(project);

        // Assert
        _projectRepositoryMock.Verify(r => r.DeleteAsync(project), Times.Once);
    }    
    
    [Test, Description("Deleting Project entity when doesn't exist throws an exception")]
    public void DeleteProject_ShouldThrowException_WhenProjectNotExists()
    {
        // arrange
        var project = new Project
        {
            Id = new Unique(Guid.NewGuid()),
            Name = "Project name",
            HoursBudget = 4000,
            IsActive = true
        };

        _projectRepositoryMock.Setup(r => r.ProjectExistsAsync(project.Id)).ReturnsAsync(false);
      
        // act
        var exception = Assert.ThrowsAsync<ProjectNotFoundException>(() => _projectService.DeleteProjectAsync(project));

        // assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.Not.Empty);
        Assert.That(exception.Message, Contains.Substring(project.Id.ToString()));
        
        _projectRepositoryMock.Verify(r => r.DeleteAsync(project), Times.Never);
    }
}