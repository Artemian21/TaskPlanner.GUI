using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Enums;

namespace TaskPlanner.DataAccess.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public Enums.PriorityStatus Priority { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
    }
}
