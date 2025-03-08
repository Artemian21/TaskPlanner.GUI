using Microsoft.AspNetCore.Mvc;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.UI.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public TaskController(ITaskService taskService, IProjectService projectService)
        {
            if (taskService == null)
            {
                throw new ArgumentNullException(nameof(taskService), "TaskService is null in TaskController!");
            }

            this._taskService = taskService;
            this._projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasks();
            return View(tasks);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpGet("Create")]
        public IActionResult Create(Guid projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Domain.Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            var project = await _projectService.GetProjectById(task.ProjectId);
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

            await _taskService.AddTask(newTask);
            return RedirectToAction("Details", "Project", new { id = task.ProjectId });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await _taskService.GetTaskById(id);
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
            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }

            var isDeleted = await _taskService.DeleteTask(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Project", new { id = task.ProjectId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var task = await _taskService.GetTaskById(id);
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

			var existingTask = await _taskService.GetTaskById(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Deadline = task.Deadline;
            existingTask.Status = task.Status;
            existingTask.Priority = task.Priority;

            var updatedTask = await _taskService.UpdateTask(existingTask.Id, existingTask.Title, existingTask.Description, existingTask.Deadline, existingTask.Status, existingTask.Priority);

            if (updatedTask == null)
            {
                return NotFound();
            }

			return RedirectToAction("Details", "Project", new { id = task.ProjectId });
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
