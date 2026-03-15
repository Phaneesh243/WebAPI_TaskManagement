using TaskTrackerWEBAPI.CoreModels.DTOs.TaskAttachments;

namespace TaskTrackerWEBAPI.CoreModels.DTOs.Tasks
{
    public class TaskDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<TaskAttachmentDto> Attachments { get; set; } = new();
    }
}
