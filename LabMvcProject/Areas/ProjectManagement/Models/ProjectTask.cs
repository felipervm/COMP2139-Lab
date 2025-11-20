using System;
using System.ComponentModel.DataAnnotations;

namespace LabMvcProject.Areas.ProjectManagement.Models
{
    public class ProjectTask
    {
        public int ProjectTaskId { get; set; }

        [Display(Name = "Task Title")]
        [StringLength(100)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
