using System.ComponentModel.DataAnnotations;

namespace TaskTrackerWEBAPI.CoreModels.DTOs.Tasks
{
    public class UpdateTaskStatusDto
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}
