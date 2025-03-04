using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskPlanner.Domain.Enums;

namespace TaskPlanner.Domain.Models
{
    public class Task
    {
        public Task() { }

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

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Not_started;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriorityStatus Priority { get; set; } = PriorityStatus.Low;
        public Guid ProjectId { get; set; }

        public static (Task? task, List<string> errors) Create(Guid id, string title, string description, DateTime? deadline, Enums.TaskStatus taskStatus, PriorityStatus priorityStatus, Guid projectId, DateTime? projectDeadline = null)
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

            if (projectDeadline.HasValue && deadline.HasValue && deadline.Value > projectDeadline.Value)
            {
                errors.Add("Task deadline cannot be later than the project's deadline.");
            }

            if (errors.Any())
            {
                return (null, errors);
            }

            return (new Task(id, title, description, deadline, taskStatus, priorityStatus, projectId), errors);
        }
    }
}
