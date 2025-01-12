using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Controllers
{
    using TodoApi.DTO;
    using TodoApi.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TodoContext _context;

        public TasksController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .Select(task => new TaskDto
                {
                    TaskId = task.TaskId,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    GroupId = task.GroupId,
                    GroupName = task.Group.Name // Dodanie nazwy grupy
                })
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(TaskDto taskDto)
        {
            var task = new Task
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                GroupId = taskDto.GroupId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Zwrot pełnego DTO z ID
            var createdTaskDto = new TaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                GroupId = task.GroupId,
                GroupName = (await _context.Groups.FindAsync(task.GroupId))?.Name
            };

            return CreatedAtAction(nameof(GetTasks), new { id = task.TaskId }, createdTaskDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskDto taskDto)
        {
            if (id != taskDto.TaskId)
            {
                return BadRequest("Task ID mismatch");
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.IsCompleted = taskDto.IsCompleted;
            task.GroupId = taskDto.GroupId;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
