using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskPlanner.Domain.Models
{
    public class Project
    {
        public Project() { }

        [JsonConstructor]
        private Project(Guid id, string name, string description, DateTime? deadline, List<Task> tasks)
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
            Deadline = deadline;
            Tasks = tasks ?? new List<Task>();
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime? Deadline { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();

        public static (Project? project, List<string> errors) Create(Guid id, string name, string description, DateTime? deadline, List<Task>? tasks = null)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Name cannot be empty.");
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

            return (new Project(id, name, description, deadline, tasks), errors);
        }
    }
}
