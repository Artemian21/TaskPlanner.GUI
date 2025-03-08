using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Entities;
using TaskPlanner.DataAccess.Repositories;
using TaskPlanner.Domain.Abstraction;

namespace TaskPlanner.DataAccess
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private TaskPlannerDBContext context;
        private IProjectRepository _projectRepository;
        private ITaskRepository _taskRepository;

        public UnitOfWork(TaskPlannerDBContext context, IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            this.context = context;
            this._projectRepository = projectRepository;
            this._taskRepository = taskRepository;
        }

        public IProjectRepository ProjectRepository
        {
            get
            {
                return _projectRepository;
            }
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                return _taskRepository;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
