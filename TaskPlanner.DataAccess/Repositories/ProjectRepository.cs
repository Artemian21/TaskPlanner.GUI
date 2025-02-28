using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Entities;

namespace TaskPlanner.DataAccess.Repositories
{
    public class ProjectRepository : IRepozitory<ProjectEntity>
    {
        private TaskPlannerDBContext context;

        public ProjectRepository(TaskPlannerDBContext context)
        {
            this.context = context;
        }

        public Task<ProjectEntity> AddAsync(ProjectEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectEntity> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProjectEntity> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectEntity> UpdateAsync(ProjectEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
