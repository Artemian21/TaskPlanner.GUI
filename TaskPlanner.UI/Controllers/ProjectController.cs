using Microsoft.AspNetCore.Mvc;
using TaskPlanner.DataAccess;
using TaskPlanner.DataAccess.Repositories;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.UI.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
            return View(projects);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            var (newProject, errors) = Project.Create(project.Id, project.Name, project.Description, project.Deadline, project.Tasks);

            if (errors.Any())
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", errors));
                return View(project);
            }

            // Якщо проект створений без помилок, зберігаємо його в базі
            await _unitOfWork.ProjectRepository.AddAsync(newProject);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var isDeleted = await _unitOfWork.ProjectRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Project project)
        {

            if (id != project.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.ProjectRepository.UpdateAsync(id, project.Name, project.Description, project.Deadline);
                return RedirectToAction(nameof(Details), new { id = project.Id });
            }
            return View(project);
        }
        //[HttpGet]
        //public async Task<IActionResult> GetAllProjects()
        //{
        //    var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
        //    return Ok(projects);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetProjectById(Guid id)
        //{
        //    var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(project);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddProject([FromBody] Project project)
        //{
        //    if (project == null)
        //    {
        //        return BadRequest("Invalid project data");
        //    }

        //    var addedProject = await _unitOfWork.ProjectRepository.AddAsync(project);
        //    return CreatedAtAction(nameof(GetProjectById), new { id = addedProject.Id }, addedProject);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProject(Guid id)
        //{
        //    var isDeleted = await _unitOfWork.ProjectRepository.DeleteAsync(id);
        //    if (!isDeleted)
        //    {
        //        return NotFound();
        //    }
        //    return NoContent();
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateProject(Guid id, [FromBody] Project project)
        //{
        //    if (project == null)
        //    {
        //        return BadRequest("Invalid project data");
        //    }

        //    var updatedProject = await _unitOfWork.ProjectRepository.UpdateAsync(id, project.Name, project.Description, project.Deadline);
        //    if (updatedProject == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(updatedProject);
        //}
    }
}
