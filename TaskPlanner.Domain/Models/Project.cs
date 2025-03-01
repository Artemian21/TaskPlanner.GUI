using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.Domain.Models
{
    public class Project
    {
        private Project(Guid id, string name, string description, DateTime? deadline)
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
            Deadline = deadline;
        }

        public Guid Id { get;}
        public string Name { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
        public DateTime? Deadline { get; }

        public static (Project? project, List<string> errors) Create(string name, string description, DateTime? deadline, List<Task>? tasks = null)
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

            return (new Project(Guid.NewGuid(), name, description, deadline), errors);
        }
    }
}
