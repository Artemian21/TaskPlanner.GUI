using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Enums;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.BusinessLogic.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TaskPlanner.Domain.Models.Task>> GetAllTasks()
        {
            return await _unitOfWork.TaskRepository.GetAllAsync();
        }

        public async Task<TaskPlanner.Domain.Models.Task> GetTaskById(Guid id)
        {
            return await _unitOfWork.TaskRepository.GetByIdAsync(id);
        }

        public async Task<TaskPlanner.Domain.Models.Task> AddTask(TaskPlanner.Domain.Models.Task task)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(task.ProjectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            return await _unitOfWork.TaskRepository.AddAsync(task);
        }

        public async Task<bool> DeleteTask(Guid id)
        {
            return await _unitOfWork.TaskRepository.DeleteAsync(id);
        }

        public async Task<TaskPlanner.Domain.Models.Task> UpdateTask(Guid id, string title, string description, DateTime? deadline, Domain.Enums.TaskStatus status, PriorityStatus priority)
        {
            return await _unitOfWork.TaskRepository.UpdateAsync(id, title, description, deadline, status, priority);
        }
    }
}
