using Microsoft.AspNetCore.Mvc;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
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

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllTask()
        {
            var tasks = await _unitOfWork.TaskRepository.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] Domain.Models.Task task)
        {
            if (task == null)
            {
                return BadRequest("Invalid task data");
            }

            var addedTask = await _unitOfWork.TaskRepository.AddAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = addedTask.Id }, addedTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var isDeleted = await _unitOfWork.TaskRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] Domain.Models.Task task)
        {
            if (task == null)
            {
                return BadRequest("Invalid task data");
            }

            var updatedTask = await _unitOfWork.TaskRepository.UpdateAsync(id, task.Title, task.Description, task.Deadline, task.Status, task.Priority);
            if (updatedTask == null)
            {
                return NotFound();
            }
            return Ok(updatedTask);
        }
    }
}
