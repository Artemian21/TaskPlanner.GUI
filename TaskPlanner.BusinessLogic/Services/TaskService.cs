using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Repositories;
using TaskPlanner.Domain.Enums;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.BusinessLogic.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            this._taskRepository = taskRepository;
            this._projectRepository = projectRepository;
        }

        public async Task<IEnumerable<TaskPlanner.Domain.Models.Task>> GetAllTasks()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<TaskPlanner.Domain.Models.Task> GetTaskById(Guid id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<TaskPlanner.Domain.Models.Task> AddTask(TaskPlanner.Domain.Models.Task task)
        {
            var project = await _projectRepository.GetByIdAsync(task.ProjectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            return await _taskRepository.AddAsync(task);
        }

        public async Task<bool> DeleteTask(Guid id)
        {
            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<TaskPlanner.Domain.Models.Task> UpdateTask(Guid id, string title, string description, DateTime deadline, Domain.Enums.TaskStatus status, PriorityStatus priority)
        {
            return await _taskRepository.UpdateAsync(id, title, description, deadline, status, priority);
        }
    }
}
