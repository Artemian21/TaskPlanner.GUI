using Microsoft.AspNetCore.Mvc;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.UI.Controllers
{
    public class TaskController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskController(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork), "UnitOfWork is null in TaskController!");
            }

            this._unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _unitOfWork.TaskRepository.GetAllAsync();
            return View(tasks);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllTask()
        //{
        //    var tasks = await _unitOfWork.TaskRepository.GetAllAsync();
        //    return Ok(tasks);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetTaskById(Guid id)
        //{
        //    var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
        //    if (task == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(task);
        //}

        [HttpGet("Create")]
        public IActionResult Create(Guid projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> AddTask([FromBody] Domain.Models.Task task)
        //{
        //    if (task == null)
        //    {
        //        return BadRequest("Invalid task data");
        //    }

        //    var addedTask = await _unitOfWork.TaskRepository.AddAsync(task);
        //    return CreatedAtAction(nameof(GetTaskById), new { id = addedTask.Id }, addedTask);
        //}

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Domain.Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(task.ProjectId);
            if (project == null)
            {
                ModelState.AddModelError(nameof(task.ProjectId), "Project does not exist.");
                return View(task);
            }

            var (newTask, errors) = Domain.Models.Task.Create(
                Guid.NewGuid(),
                task.Title,
                task.Description,
                task.Deadline,
                task.Status,
                task.Priority,
                task.ProjectId,
                project.Deadline
            );

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(task);
            }

            await _unitOfWork.TaskRepository.AddAsync(newTask);
            return RedirectToAction("Details", "Project", new { id = task.ProjectId });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var isDeleted = await _unitOfWork.TaskRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Project", new { id = task.ProjectId });
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTask(Guid id)
        //{
        //    var isDeleted = await _unitOfWork.TaskRepository.DeleteAsync(id);
        //    if (!isDeleted)
        //    {
        //        return NotFound();
        //    }
        //    return NoContent();
        //}

        public async Task<IActionResult> Edit(Guid id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Domain.Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

			var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Deadline = task.Deadline;
            existingTask.Status = task.Status;
            existingTask.Priority = task.Priority;

            var updatedTask = await _unitOfWork.TaskRepository.UpdateAsync(existingTask.Id, existingTask.Title, existingTask.Description, existingTask.Deadline, existingTask.Status, existingTask.Priority);

            if (updatedTask == null)
            {
                return NotFound();
            }

			return RedirectToAction("Details", "Project", new { id = task.ProjectId });
        }


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateTask(Guid id, [FromBody] Domain.Models.Task task)
        //{
        //    if (task == null)
        //    {
        //        return BadRequest("Invalid task data");
        //    }

        //    var updatedTask = await _unitOfWork.TaskRepository.UpdateAsync(id, task.Title, task.Description, task.Deadline, task.Status, task.Priority);
        //    if (updatedTask == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(updatedTask);
        //}
    }
}
