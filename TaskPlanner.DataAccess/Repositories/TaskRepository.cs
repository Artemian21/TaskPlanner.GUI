using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Entities;
using TaskPlanner.Domain.Enums;

namespace TaskPlanner.DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private TaskPlannerDBContext context;

        public TaskRepository(TaskPlannerDBContext context)
        {
            this.context = context;
        }

        public async Task<Domain.Models.Task> AddAsync(Domain.Models.Task task)
        {
            var taskEntity = new TaskEntity
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = DateTime.Now,
                Deadline = task.Deadline,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId
            };

            await context.Tasks.AddAsync(taskEntity);
            await context.SaveChangesAsync();

            return Domain.Models.Task.Create(taskEntity.Id, taskEntity.Title, taskEntity.Description, taskEntity.Deadline, taskEntity.Status, taskEntity.Priority, taskEntity.ProjectId).task;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await context.Tasks.Where(t => t.Id == id).ExecuteDeleteAsync();
            return true;
        }

        public async Task<IEnumerable<Domain.Models.Task>> GetAllAsync()
        {
            var taskEntities = await context.Tasks.AsNoTracking().ToListAsync();

            var tasks = taskEntities.Select(t => Domain.Models.Task.Create(t.Id, t.Title, t.Description, t.Deadline, t.Status, t.Priority, t.ProjectId).task)
                .Where(task => task != null)
                .ToList();

            return tasks;
        }

        public async Task<Domain.Models.Task> GetByIdAsync(Guid id)
        {
            var taskEntity = await context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

            if (taskEntity == null)
            {
                return null;
            }

            return Domain.Models.Task.Create(taskEntity.Id, taskEntity.Title, taskEntity.Description, taskEntity.Deadline, taskEntity.Status, taskEntity.Priority, taskEntity.ProjectId).task;
        }

        public async Task<Domain.Models.Task> UpdateAsync(Guid id, string title, string description, DateTime deadline, Domain.Enums.TaskStatus status, PriorityStatus priority)
        {
            await context.Tasks.Where(t => t.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.Title, title)
                    .SetProperty(t => t.Description, description)
                    .SetProperty(t => t.Deadline, deadline)
                    .SetProperty(t => t.Status, status)
                    .SetProperty(t => t.Priority, priority));

            var updatedTask = await context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (updatedTask == null)
            {
                return null;
            }
            return Domain.Models.Task.Create(updatedTask.Id, updatedTask.Title, updatedTask.Description, updatedTask.Deadline, updatedTask.Status, updatedTask.Priority, updatedTask.ProjectId).task;
        }
    }
}
