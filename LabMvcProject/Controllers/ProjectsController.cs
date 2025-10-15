using Microsoft.AspNetCore.Mvc;
using LabMvcProject.Models;
using Microsoft.EntityFrameworkCore;
using LabMvcProject.Data;
using System.Linq;

namespace LabMvcProject.Controllers
{
    
    [Route("projects")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        [HttpGet("")]
        public IActionResult Index()
        {
            var projects = _context.Projects.ToList();
             ViewBag.Searched = false;
            return View(projects);
        }

       
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? term)
        {
            var projects = from p in _context.Projects select p;
            bool searched = false;

            if (!string.IsNullOrEmpty(term))
            {
                searched = true;
                projects = projects.Where(p =>
                    p.Name.Contains(term) ||
                    p.Description.Contains(term));
            }

            var result = await projects.ToListAsync();
            ViewBag.SearchTerm = term;
            ViewBag.Searched = searched;

            if (searched && !result.Any())
            {
                ViewBag.Message = $"No projects found matching '{term}'.";
            }

            return View("Index", result);
        }

        
        [HttpGet("details/{id:int}")]
        public IActionResult Details(int id)
        {
            var project = _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (!ModelState.IsValid)
            {
                return View(project);
            }

            project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
            project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);
            project.Tasks = new List<ProjectTask>();

            _context.Projects.Add(project);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

      
        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

       
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectId,Name,Description,StartDate,EndDate,Status")] Project project)
        {
            if (id != project.ProjectId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
                    project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);

                    _context.Update(project);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Projects.Any(e => e.ProjectId == project.ProjectId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        
        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

       
        [HttpPost("delete/{id:int}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var project = _context.Projects.Find(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

       
        [HttpGet("seed")]
        public IActionResult SeedTestProject()
        {
            var project = new Project
            {
                Name = "Test Project",
                Description = "This is a test project",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Status = "Active",
                Tasks = new List<ProjectTask>()
            };

            _context.Projects.Add(project);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
