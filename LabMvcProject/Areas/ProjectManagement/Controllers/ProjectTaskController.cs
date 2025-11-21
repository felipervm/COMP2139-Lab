using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabMvcProject.Data;
using LabMvcProject.Areas.ProjectManagement.Models;

namespace LabMvcProject.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("ProjectManagement/[controller]")]
    public class ProjectTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectManagement/ProjectTask?projectId=1
        [HttpGet("")]
        public async Task<IActionResult> Index(int projectId, string? term)
        {
            var query = _context.ProjectTasks
                .Where(t => t.ProjectId == projectId);

            ViewBag.ProjectId = projectId;

            if (!string.IsNullOrWhiteSpace(term))
            {
                ViewBag.Searched = true;
                ViewBag.SearchTerm = term;

                query = query.Where(t =>
                    t.Title.Contains(term) ||
                    t.Description.Contains(term));
            }

            return View(await query.ToListAsync());
        }

        // GET: ProjectManagement/ProjectTask/Details/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // GET: ProjectManagement/ProjectTask/Create?projectId=1
        [HttpGet("create")]
        public IActionResult Create(int projectId)
        {
            var task = new ProjectTask { ProjectId = projectId };
            return View(task);
        }

        // POST: ProjectManagement/ProjectTask/Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(ProjectTask model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _context.ProjectTasks.AddAsync(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { projectId = model.ProjectId });
        }

        // GET: ProjectManagement/ProjectTask/Edit/5
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // POST: ProjectManagement/ProjectTask/Edit/5
        [HttpPost("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, ProjectTask model)
        {
            if (id != model.ProjectTaskId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { projectId = model.ProjectId });
        }

        // POST: ProjectManagement/ProjectTask/Delete/5
        [HttpPost("delete/{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);

            if (task == null)
                return NotFound();

            int projectId = task.ProjectId;

            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { projectId });
        }
    }
}
