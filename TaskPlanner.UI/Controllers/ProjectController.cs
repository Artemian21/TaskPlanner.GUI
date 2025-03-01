using Microsoft.AspNetCore.Mvc;
using TaskPlanner.DataAccess.Repositories;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectRepository.GetAllAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest("Invalid project data");
            }

            var addedProject = await _projectRepository.AddAsync(project);
            return CreatedAtAction(nameof(GetProjectById), new { id = addedProject.Id }, addedProject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var isDeleted = await _projectRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest("Invalid project data");
            }

            var updatedProject = await _projectRepository.UpdateAsync(id, project.Name, project.Description, project.Deadline);
            if (updatedProject == null)
            {
                return NotFound();
            }
            return Ok(updatedProject);
        }
    }
}
