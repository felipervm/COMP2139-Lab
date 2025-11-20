using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabMvcProject.Data;
using LabMvcProject.Areas.ProjectManagement.Models;


namespace LabMvcProject.Areas.ProjectManagement.Controllers
{    
    [Area("ProjectManagement")]
    [Route("Projects/{projectId:int}/Tasks")]
    public class ProjectTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects/{projectId}/Tasks
        [HttpGet("")]
        public async Task<IActionResult> Index(int projectId, string? term)
        {
            var tasks = _context.ProjectTasks
                .Include(t => t.Project)
                .Where(t => t.ProjectId == projectId);

            bool searched = false;

            if (!string.IsNullOrWhiteSpace(term))
            {
                searched = true;
                tasks = tasks.Where(t => t.Title.Contains(term) || t.Description.Contains(term));
            }

            var result = await tasks.ToListAsync();

            ViewBag.ProjectId = projectId;
            ViewBag.SearchTerm = term ?? "";
            ViewBag.Searched = searched;

            return View(result);
        }

        // GET: Projects/{projectId}/Tasks/Create
        [HttpGet("Create")]
        public IActionResult Create(int projectId)
        {
            return View(new ProjectTask { ProjectId = projectId });
        }

        // POST: Projects/{projectId}/Tasks/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int projectId, ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                projectTask.ProjectId = projectId;
                _context.Add(projectTask);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Project", new { id = projectId });
            }

            ViewBag.ProjectId = projectId;
            return View(projectTask);
        }

        // GET: Projects/{projectId}/Tasks/Details/{id}
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null) return NotFound();

            return View(task);
        }

        // GET: Projects/{projectId}/Tasks/Edit/{id}
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task == null) return NotFound();

            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        // POST: Projects/{projectId}/Tasks/Edit/{id}
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId,Title,Description,ProjectId")] ProjectTask projectTask)
        {
            if (id != projectTask.ProjectTaskId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ProjectTasks.Any(e => e.ProjectTaskId == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Details", "Project", new { id = projectTask.ProjectId });
            }

            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", projectTask.ProjectId);
            return View(projectTask);
        }

        // GET: Projects/{projectId}/Tasks/Delete/{id}
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null) return NotFound();

            return View(task);
        }

        // POST: Projects/{projectId}/Tasks/Delete/{id}
        [HttpPost("Delete/{id:int}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task != null)
            {
                _context.ProjectTasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Project", new { id = task.ProjectId });
        }
    }
}
