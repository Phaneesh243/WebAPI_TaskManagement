using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerWEBAPI.CoreModels.Entity
{
    [Table("TaskAttachments")]
    public class TaskAttachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string FileUrl { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ContentType { get; set; }

        public DateTime UploadedAt { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("TaskId")]
        public TaskItem? TaskItem { get; set; }
    }
}
