using TaskPlanner.Domain.Models;

namespace TaskPlanner.Domain.Abstraction
{
    public interface IProjectService
    {
        Task<Project> AddProject(Project project);
        Task<bool> DeleteProject(Guid id);
        Task<IEnumerable<Project>> GetAllProjects();
        Task<Project> GetProjectById(Guid id);
        Task<Project> UpdateProject(Guid id, string name, string decription, DateTime deadline);
    }
}