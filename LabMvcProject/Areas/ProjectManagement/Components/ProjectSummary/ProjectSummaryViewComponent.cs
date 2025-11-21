using Microsoft.AspNetCore.Mvc;
using LabMvcProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LabMvcProject.Components.ProjectSummary
{
    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ProjectSummaryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks) // if you have tasks
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            return View(project); // This goes to Default.cshtml
        }
    }
}
