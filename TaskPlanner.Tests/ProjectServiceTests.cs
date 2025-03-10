using AutoFixture;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.BusinessLogic.Services;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.Tests
{
    public class ProjectServiceTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IProjectService _projectService;
        private readonly IProjectRepository _projectRepositoryMock;
        private readonly Fixture _fixture;

        public ProjectServiceTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _projectRepositoryMock = Substitute.For<IProjectRepository>();
            _unitOfWorkMock.ProjectRepository.Returns(_projectRepositoryMock);
            _projectService = new ProjectService(_unitOfWorkMock);
            _fixture = new Fixture();
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllProjects_ShouldReturnAllProjects()
        {
            var projects = _fixture.CreateMany<Project>().ToList();
            _projectRepositoryMock.GetAllAsync().Returns(projects);

            var result = await _projectService.GetAllProjects();

            Assert.NotNull(result);
            Assert.Equal(projects, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjectById_ShouldReturnProject()
        {
            var project = _fixture.Create<Project>();
            _projectRepositoryMock.GetByIdAsync(Arg.Any<Guid>()).Returns(project);

            var result = await _projectService.GetProjectById(project.Id);

            Assert.NotNull(result);
            Assert.Equal(project, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjectById_ShouldReturnNull_WhenProjectNotFound()
        {
            _projectRepositoryMock.GetByIdAsync(Arg.Any<Guid>()).Returns((Project)null);
            var result = await _projectService.GetProjectById(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddProject_ShouldReturnProject()
        {
            var project = _fixture.Create<Project>();
            _projectRepositoryMock.AddAsync(Arg.Any<Project>()).Returns(project);

            var result = await _projectService.AddProject(project);

            Assert.NotNull(result);
            Assert.Equal(project, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteProject_ShouldReturnTrue()
        {
            var projectId = Guid.NewGuid();
            _projectRepositoryMock.DeleteAsync(Arg.Any<Guid>()).Returns(true);

            var result = await _projectService.DeleteProject(projectId);

            Assert.True(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteProject_ShouldReturnFalse_WhenProjectNotFound()
        {
            var projectId = Guid.NewGuid();
            _projectRepositoryMock.DeleteAsync(Arg.Any<Guid>()).Returns(false);

            var result = await _projectService.DeleteProject(projectId);

            Assert.False(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProject_ShouldReturnProject()
        {
            var project = _fixture.Create<Project>();
            _projectRepositoryMock.UpdateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime?>()).Returns(project);

            var result = await _projectService.UpdateProject(project.Id, project.Name, project.Description, project.Deadline);

            Assert.NotNull(result);
            Assert.Equal(project, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProject_ShouldReturnNull_WhenProjectNotFound()
        {
            _projectRepositoryMock.UpdateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime?>()).Returns((Project)null);

            var result = await _projectService.UpdateProject(Guid.NewGuid(), "name", "description", DateTime.Now);

            Assert.Null(result);
        }
    }
}
