using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Controllers
{
    using DTO;
    using Models;

    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly TodoContext _context;

        public GroupsController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroups()
        {
            var groups = await _context.Groups
                .Include(g => g.Tasks)
                .Select(group => new GroupDto
                {
                    GroupId = group.GroupId,
                    Name = group.Name,
                    Tasks = group.Tasks.Select(task => new TaskDto
                    {
                        TaskId = task.TaskId,
                        Title = task.Title,
                        Description = task.Description,
                        IsCompleted = task.IsCompleted,
                        GroupId = task.GroupId
                    }).ToList()
                })
                .ToListAsync();

            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult<GroupDto>> CreateGroup(GroupDto groupDto)
        {
            var group = new Group
            {
                Name = groupDto.Name
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            var createdGroupDto = new GroupDto
            {
                GroupId = group.GroupId,
                Name = group.Name,
                Tasks = new List<TaskDto>() // Nowa grupa nie ma jeszcze zadań
            };

            return CreatedAtAction(nameof(GetGroups), new { id = group.GroupId }, createdGroupDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, GroupDto groupDto)
        {
            if (id != groupDto.GroupId)
            {
                return BadRequest("Group ID mismatch");
            }

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            group.Name = groupDto.Name;

            _context.Groups.Update(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _context.Groups.Include(g => g.Tasks).FirstOrDefaultAsync(g => g.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            if (group.Tasks.Any())
            {
                return BadRequest("Cannot delete a group that has tasks.");
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
