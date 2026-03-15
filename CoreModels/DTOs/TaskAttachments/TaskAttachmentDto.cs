namespace TaskTrackerWEBAPI.CoreModels.DTOs.TaskAttachments
{
    public class TaskAttachmentDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? ContentType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
