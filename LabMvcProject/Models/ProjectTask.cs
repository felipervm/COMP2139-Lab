using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [ForeignKey("Project")]
    public int ProjectId { get; set; }

    public Project? Project { get; set; }
}

