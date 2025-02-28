using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Entities;

namespace TaskPlanner.DataAccess
{
    public class TaskPlannerDBContext : DbContext
    {
        public TaskPlannerDBContext(DbContextOptions<TaskPlannerDBContext> options) : base(options)
        {

        }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
    }
}
