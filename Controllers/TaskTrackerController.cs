using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerWEBAPI.CoreModels;
using TaskTrackerWEBAPI.CoreModels.DTOs.Messages;
using TaskTrackerWEBAPI.CoreModels.DTOs.TaskAttachments;
using TaskTrackerWEBAPI.CoreModels.DTOs.Tasks;
using TaskTrackerWEBAPI.CoreModels.Entity;

namespace TaskTrackerWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTrackerController : ControllerBase
    {
        private readonly TaskTrackerDbContext _context;

        public TaskTrackerController(TaskTrackerDbContext context)
        {
            _context = context;
        }

        // 1. POST: api/TaskTracker/tasks
        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!IsValidPriority(dto.Priority))
                return BadRequest(new
                {
                    message = "Invalid priority. Allowed values: Low, Medium, High"
                });

            var taskItem = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = "Pending",
                DueDate = dto.DueDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                IsActive = true
            };

            _context.Tasks.Add(taskItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Task created successfully",
                data = new
                {
                    taskItem.Id,
                    taskItem.Title,
                    taskItem.Description,
                    taskItem.Status,
                    taskItem.Priority,
                    taskItem.DueDate,
                    taskItem.CreatedAt
                }
            });
        }

        // 2. GET: api/TaskTracker/tasks
        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.Tasks
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TaskListDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    Priority = t.Priority,
                    DueDate = t.DueDate,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(tasks);
        }

        // 3. GET: api/TaskTracker/tasks/{id}
        [HttpGet("tasks/{id:int}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.TaskAttachments.Where(a => a.IsActive))
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

            if (task == null)
            {
                return NotFound(new
                {
                    message = $"Task with Id {id} not found"
                });
            }

            var result = new TaskDetailDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                Attachments = task.TaskAttachments.Select(a => new TaskAttachmentDto
                {
                    Id = a.Id,
                    TaskId = a.TaskId,
                    FileName = a.FileName,
                    FileUrl = a.FileUrl,
                    ContentType = a.ContentType,
                    UploadedAt = a.UploadedAt
                }).ToList()
            };

            return Ok(result);
        }

        // 4. PUT: api/TaskTracker/tasks/{id}/status
        [HttpPut("tasks/{id:int}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!IsValidStatus(dto.Status))
            {
                return BadRequest(new
                {
                    message = "Invalid status. Allowed values: Pending, InProgress, Completed"
                });
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

            if (task == null)
            {
                return NotFound(new
                {
                    message = $"Task with Id {id} not found"
                });
            }

            task.Status = dto.Status;
            task.UpdatedAt = DateTime.Now;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Task status updated successfully",
                data = new
                {
                    task.Id,
                    task.Title,
                    task.Status,
                    task.UpdatedAt
                }
            });
        }

        // 5. POST: api/TaskTracker/tasks/{id}/attachment
        [HttpPost("tasks/{id:int}/attachment")]
        public async Task<IActionResult> UploadTaskAttachment(int id, [FromBody] UploadTaskAttachmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

            if (task == null)
            {
                return NotFound(new
                {
                    message = $"Task with Id {id} not found"
                });
            }

            var attachment = new TaskAttachment
            {
                TaskId = id,
                FileName = dto.FileName,
                FileUrl = dto.FileUrl,
                ContentType = dto.ContentType,
                UploadedAt = DateTime.Now,
                IsActive = true
            };

            _context.TaskAttachments.Add(attachment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Attachment uploaded successfully",
                data = new
                {
                    attachment.Id,
                    attachment.TaskId,
                    attachment.FileName,
                    attachment.FileUrl,
                    attachment.ContentType,
                    attachment.UploadedAt
                }
            });
        }

        // 6. POST: api/TaskTracker/messages
        [HttpPost("messages")]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = new Message
            {
                Name = dto.Name,
                Email = dto.Email,
                Subject = dto.Subject,
                MessageText = dto.MessageText,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            var response = new MessageResponseDto
            {
                Id = message.Id,
                Name = message.Name,
                Email = message.Email,
                Subject = message.Subject,
                MessageText = message.MessageText,
                CreatedAt = message.CreatedAt
            };

            return Ok(new
            {
                message = "Message submitted successfully",
                data = response
            });
        }

        private bool IsValidPriority(string priority)
        {
            var allowedPriorities = new[] { "Low", "Medium", "High" };
            return allowedPriorities.Contains(priority);
        }

        private bool IsValidStatus(string status)
        {
            var allowedStatuses = new[] { "Pending", "InProgress", "Completed" };
            return allowedStatuses.Contains(status);
        }
    }
}