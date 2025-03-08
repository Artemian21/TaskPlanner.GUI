using TaskPlanner.Domain.Enums;

namespace TaskPlanner.Domain.Abstraction
{
    public interface ITaskService
    {
        Task<Domain.Models.Task> AddTask(Domain.Models.Task task);
        Task<bool> DeleteTask(Guid id);
        Task<IEnumerable<Domain.Models.Task>> GetAllTasks();
        Task<Domain.Models.Task> GetTaskById(Guid id);
        Task<Domain.Models.Task> UpdateTask(Guid id, string title, string description, DateTime? deadline, Domain.Enums.TaskStatus status, PriorityStatus priority);
    }
}