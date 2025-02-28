using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Entities;

namespace TaskPlanner.DataAccess.Repositories
{
    public class TaskRepository : IRepozitory<TaskEntity>
    {
        private TaskPlannerDBContext context;

        public TaskRepository(TaskPlannerDBContext context)
        {
            this.context = context;
        }

        public Task<TaskEntity> AddAsync(TaskEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TaskEntity> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TaskEntity> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TaskEntity> UpdateAsync(TaskEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
