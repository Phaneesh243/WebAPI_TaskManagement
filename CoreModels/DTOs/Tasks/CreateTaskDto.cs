using System.ComponentModel.DataAnnotations;

namespace TaskTrackerWEBAPI.CoreModels.DTOs.Tasks
{
    public class CreateTaskDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Priority { get; set; } = "Medium";

        public DateTime? DueDate { get; set; }
    }
}
