using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabMvcProject.Areas.ProjectManagement.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Display(Name = "Project Name")]
        [StringLength(80, ErrorMessage = "Name cannot exceed 80 characters.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public List<ProjectTask> Tasks { get; set; }
    }
}
