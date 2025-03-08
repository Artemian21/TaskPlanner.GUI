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
        private readonly IUnitOfWork _projectRepository;
        public ProjectService(IUnitOfWork projectRepository)
        {
            this._projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _projectRepository.ProjectRepository.GetAllAsync();
        }

        public async Task<Project> GetProjectById(Guid id)
        {
            return await _projectRepository.ProjectRepository.GetByIdAsync(id);
        }

        public async Task<Project> AddProject(Project project)
        {
            return await _projectRepository.ProjectRepository.AddAsync(project);
        }

        public async Task<bool> DeleteProject(Guid id)
        {
            return await _projectRepository.ProjectRepository.DeleteAsync(id);
        }

        public async Task<Project> UpdateProject(Guid id, string name, string decription, DateTime? deadline)
        {
            return await _projectRepository.ProjectRepository.UpdateAsync(id, name, decription, deadline);
        }
    }
}
