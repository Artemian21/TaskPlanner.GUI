using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Repositories;

namespace TaskPlanner.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private TaskPlannerDBContext context;
        private ProjectRepository projectRepository;
        private TaskRepository taskRepository;

        public UnitOfWork(TaskPlannerDBContext context)
        {
            this.context = context;
        }

        public ProjectRepository ProjectRepository
        {
            get
            {
                if (projectRepository == null)
                {
                    projectRepository = new ProjectRepository(context);
                }
                return projectRepository;
            }
        }

        public TaskRepository TaskRepository
        {
            get
            {
                if (taskRepository == null)
                {
                    taskRepository = new TaskRepository(context);
                }
                return taskRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
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
