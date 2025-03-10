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
using TaskPlanner.Domain.Enums;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.Tests
{
    public class TaskServiceTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly ITaskService _taskService;
        private readonly ITaskRepository _taskRepositoryMock;
        private readonly Fixture _fixture;

        public TaskServiceTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _taskRepositoryMock = Substitute.For<ITaskRepository>();
            _unitOfWorkMock.TaskRepository.Returns(_taskRepositoryMock);
            _taskService = new TaskService(_unitOfWorkMock);
            _fixture = new Fixture();
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ShouldReturnAllTasks()
        {
            var tasks = _fixture.CreateMany<TaskPlanner.Domain.Models.Task>().ToList();
            _taskRepositoryMock.GetAllAsync().Returns(tasks);

            var result = await _taskService.GetAllTasks();

            Assert.NotNull(result);
            Assert.Equal(tasks, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskById_ShouldReturnTask()
        {
            var task = _fixture.Create<TaskPlanner.Domain.Models.Task>();
            _taskRepositoryMock.GetByIdAsync(Arg.Any<Guid>()).Returns(task);

            var result = await _taskService.GetTaskById(task.Id);

            Assert.NotNull(result);
            Assert.Equal(task, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskById_ShouldReturnNull_WhenTaskNotFound()
        {
            var task = _fixture.Create<TaskPlanner.Domain.Models.Task>();
            _taskRepositoryMock.GetByIdAsync(Arg.Any<Guid>()).Returns((TaskPlanner.Domain.Models.Task)null);

            var result = await _taskService.GetTaskById(task.Id);

            Assert.Null(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddTask_ShouldReturnTask()
        {
            var task = _fixture.Create<TaskPlanner.Domain.Models.Task>();
            var project = _fixture.Build<Project>()
                          .With(p => p.Id, task.ProjectId)
                          .Create();
            _unitOfWorkMock.ProjectRepository.GetByIdAsync(task.ProjectId).Returns(project);
            _unitOfWorkMock.TaskRepository.AddAsync(Arg.Any<TaskPlanner.Domain.Models.Task>()).Returns(task);

            var result = await _taskService.AddTask(task);

            Assert.NotNull(result);
            Assert.Equal(task, result);
        }


        [Fact]
        public async System.Threading.Tasks.Task AddTask_ShouldThrowException_WhenProjectNotFound()
        {
            var task = _fixture.Create<TaskPlanner.Domain.Models.Task>();
            _unitOfWorkMock.ProjectRepository.GetByIdAsync(task.ProjectId).Returns((Project)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.AddTask(task));
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ShouldReturnTrue()
        {
            var task = _fixture.Create<TaskPlanner.Domain.Models.Task>();
            _taskRepositoryMock.DeleteAsync(Arg.Any<Guid>()).Returns(true);

            var result = await _taskService.DeleteTask(task.Id);

            Assert.True(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ShouldReturnFalse_WhenTaskNotFound()
        {
            var task = _fixture.Create<TaskPlanner.Domain.Models.Task>();
            _taskRepositoryMock.DeleteAsync(Arg.Any<Guid>()).Returns(false);

            var result = await _taskService.DeleteTask(task.Id);

            Assert.False(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_ShouldReturnTask()
        {
            var task = _fixture.Create<TaskPlanner.Domain.Models.Task>();
            _taskRepositoryMock.UpdateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime?>(), Arg.Any<Domain.Enums.TaskStatus>(), Arg.Any<PriorityStatus>()).Returns(task);

            var result = await _taskService.UpdateTask(task.Id, task.Title, task.Description, task.Deadline, task.Status, task.Priority);

            Assert.NotNull(result);
            Assert.Equal(task, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_ShouldReturnNull_WhenTaskNotFound()
        {
            var task = _fixture.Create<TaskPlanner.Domain.Models.Task>();
            _taskRepositoryMock.UpdateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime?>(), Arg.Any<Domain.Enums.TaskStatus>(), Arg.Any<PriorityStatus>()).Returns((TaskPlanner.Domain.Models.Task)null);

            var result = await _taskService.UpdateTask(task.Id, task.Title, task.Description, task.Deadline, task.Status, task.Priority);

            Assert.Null(result);
        }
    }
}
