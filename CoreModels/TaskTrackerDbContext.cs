using Microsoft.EntityFrameworkCore;
using TaskTrackerWEBAPI.CoreModels.Entity;

namespace TaskTrackerWEBAPI.CoreModels
{
    public class TaskTrackerDbContext :DbContext
    {
        public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options)
            : base(options)
        {
        }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
