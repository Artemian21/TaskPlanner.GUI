namespace TaskPlanner.Domain.Abstraction
{
    public interface IUnitOfWork
    {
        IProjectRepository ProjectRepository { get; }
        ITaskRepository TaskRepository { get; }

        void Dispose();
        void Dispose(bool disposing);
        Task SaveAsync();
    }
}