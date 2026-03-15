using System.ComponentModel.DataAnnotations;

namespace TaskTrackerWEBAPI.CoreModels.DTOs.Tasks
{
    public class UploadTaskAttachmentDto
    {
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string FileUrl { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ContentType { get; set; }
    }
}
