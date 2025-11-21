using System;
using System.ComponentModel.DataAnnotations;

namespace LabMvcProject.Areas.ProjectManagement.Models
{
    public class ProjectComment
    {
        [Key]
        public int CommentId { get; set; } 
        
        [Required]
        public string Content { get; set; }

        public int ProjectId { get; set; }  

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Project Project { get; set; }
    }
}
