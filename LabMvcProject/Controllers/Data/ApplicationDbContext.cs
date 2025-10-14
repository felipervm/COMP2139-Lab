using Microsoft.EntityFrameworkCore;
using LabMvcProject.Models; // Replace with your actual namespace

namespace LabMvcProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        // public DbSet<Task> Tasks { get; set; } // If adding Tasks entity
        
        public DbSet<ProjectTask> ProjectTasks { get; set; }

    }

}


