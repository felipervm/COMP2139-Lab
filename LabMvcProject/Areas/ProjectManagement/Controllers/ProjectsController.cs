using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabMvcProject.Data;
using LabMvcProject.Areas.ProjectManagement.Models;

namespace LabMvcProject.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("ProjectManagement/[controller]")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectManagement/Project
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects.ToListAsync();
            ViewBag.Searched = false;
            return View(projects);
        }

        // GET: ProjectManagement/Project/Search
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? term)
        {
            var query = _context.Projects.AsQueryable();
            bool searched = false;

            if (!string.IsNullOrWhiteSpace(term))
            {
                searched = true;
                query = query.Where(p =>
                    p.Name.Contains(term) ||
                    p.Description.Contains(term));
            }

            ViewBag.Searched = searched;
            ViewBag.SearchTerm = term;

            return View("Index", await query.ToListAsync());
        }

        // GET: ProjectManagement/Project/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
                return NotFound();

            return View(project);
        }

        // GET: ProjectManagement/Project/Create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectManagement/Project/Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(Project model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _context.Projects.AddAsync(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: ProjectManagement/Project/Edit/5
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            return View(project);
        }

        // POST: ProjectManagement/Project/Edit/5
        [HttpPost("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, Project model)
        {
            if (id != model.ProjectId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // POST: ProjectManagement/Project/Delete/5
        [HttpPost("delete/{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
