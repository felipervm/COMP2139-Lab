using Microsoft.AspNetCore.Mvc;
using LabMvcProject.Models; 
using Microsoft.EntityFrameworkCore;
using LabMvcProject.Data;

public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Project/Index
    [HttpGet]
    public IActionResult Index()
    {
        var projects = _context.Projects.ToList();
        return View(projects);
    }

    // GET: Project/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Project/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Project project)
    {
        Console.WriteLine("=== CREATE METHOD CALLED ===");
        Console.WriteLine($"Name: {project.Name}");
        Console.WriteLine($"Description: {project.Description}");
        Console.WriteLine($"StartDate: {project.StartDate}");
        Console.WriteLine($"EndDate: {project.EndDate}");
        Console.WriteLine($"Status: {project.Status}");
        Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("=== MODEL STATE ERRORS ===");
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                if (errors.Any())
                {
                    Console.WriteLine($"Key: {key}");
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"  Error: {error.ErrorMessage}");
                        if (error.Exception != null)
                        {
                            Console.WriteLine($"  Exception: {error.Exception.Message}");
                        }
                    }
                }
            }
            return View(project);
        }

        try
        {
            // Convert dates to UTC for database compatibility
            project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
            project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);
            
            // Initialize Tasks collection
            project.Tasks = new List<ProjectTask>();
            
            Console.WriteLine("=== ADDING TO DATABASE ===");
            _context.Projects.Add(project);
            
            Console.WriteLine("=== SAVING CHANGES ===");
            var result = _context.SaveChanges();
            Console.WriteLine($"=== SAVED! Rows affected: {result} ===");
            Console.WriteLine($"Project ID after save: {project.ProjectId}");
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Console.WriteLine("=== EXCEPTION OCCURRED ===");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            ModelState.AddModelError("", "Unable to save changes: " + ex.Message);
            return View(project);
        }
    }

    // GET: Project/Details/5
    [HttpGet]
    public IActionResult Details(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    // GET: Project/Edit/5
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    // POST: Project/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectId,Name,Description,StartDate,EndDate,Status")] Project project)
    {
        if (id != project.ProjectId)
        {
            return NotFound();
        }

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
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }
        return View(project);
    }

    // GET: Project/Delete/5
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    // POST: Project/Delete/5
    [HttpPost, ActionName("Delete")]
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
    [HttpGet]
public IActionResult SeedTestProject()
{
    var project = new Project
    {
        Name = "Test Project",
        Description = "This is a test",
        StartDate = DateTime.UtcNow,
        EndDate = DateTime.UtcNow.AddDays(30),
        Status = "Active",
        Tasks = new List<ProjectTask>()
    };

    _context.Projects.Add(project);
    _context.SaveChanges();

    return RedirectToAction("Index");
}

}