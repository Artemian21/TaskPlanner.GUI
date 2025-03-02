using TaskPlanner.Domain.Enums;

namespace TaskPlanner.Domain.Abstraction
{
    public interface ITaskRepository
    {
        Task<Domain.Models.Task> AddAsync(Domain.Models.Task task);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Domain.Models.Task>> GetAllAsync();
        Task<Domain.Models.Task> GetByIdAsync(Guid id);
        Task<Domain.Models.Task> UpdateAsync(Guid id, string title, string description, DateTime? deadline, Domain.Enums.TaskStatus status, PriorityStatus priority);
    }
}