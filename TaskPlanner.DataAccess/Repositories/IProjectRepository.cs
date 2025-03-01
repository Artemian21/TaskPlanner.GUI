using TaskPlanner.Domain.Models;

namespace TaskPlanner.DataAccess.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> AddAsync(Project project);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> GetByIdAsync(Guid id);
        Task<Project> UpdateAsync(Guid id, string name, string decription, DateTime deadline);
    }
}