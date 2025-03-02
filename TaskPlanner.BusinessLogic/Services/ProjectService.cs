using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.BusinessLogic.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectService(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project> GetProjectById(Guid id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task<Project> AddProject(Project project)
        {
            return await _projectRepository.AddAsync(project);
        }

        public async Task<bool> DeleteProject(Guid id)
        {
            return await _projectRepository.DeleteAsync(id);
        }

        public async Task<Project> UpdateProject(Guid id, string name, string decription, DateTime deadline)
        {
            return await _projectRepository.UpdateAsync(id, name, decription, deadline);
        }
    }
}
