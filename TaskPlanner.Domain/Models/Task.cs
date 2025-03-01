using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.Domain.Enums;

namespace TaskPlanner.Domain.Models
{
    public class Task
    {
        public Task(Guid id, string title, string description, DateTime? deadline, Enums.TaskStatus status, PriorityStatus priority, Guid projectId)
        {
            Id = id;
            Title = title;
            Description = description;
            CreatedAt = DateTime.Now;
            Deadline = deadline;
            Status = status;
            Priority = priority;
            ProjectId = projectId;
        }

        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
        public DateTime? Deadline { get; }
        public Enums.TaskStatus Status { get; } = Enums.TaskStatus.Not_started;
        public PriorityStatus Priority { get; } = PriorityStatus.Low;
        public Guid ProjectId { get; }

        public static (Task? task, List<string> errors) Create(string title, string description, DateTime? deadline, Enums.TaskStatus taskStatus, PriorityStatus priorityStatus, Guid projectId)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(title))
            {
                errors.Add("Title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                errors.Add("Description cannot be empty.");
            }

            if (deadline.HasValue && deadline.Value < DateTime.UtcNow)
            {
                errors.Add("Deadline cannot be in the past.");
            }

            if (errors.Any())
            {
                return (null, errors);
            }

            return (new Task(Guid.NewGuid(), title, description, deadline, taskStatus, priorityStatus, projectId), errors);
        }
    }
}
