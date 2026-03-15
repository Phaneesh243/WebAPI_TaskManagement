using System.ComponentModel.DataAnnotations;

namespace TaskTrackerWEBAPI.CoreModels.DTOs.Messages
{
    public class CreateMessageDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string MessageText { get; set; } = string.Empty;
    }
}
