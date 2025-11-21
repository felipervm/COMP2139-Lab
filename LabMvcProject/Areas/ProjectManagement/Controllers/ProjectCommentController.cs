using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabMvcProject.Data;
using LabMvcProject.Areas.ProjectManagement.Models;

namespace LabMvcProject.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("ProjectManagement/Comments")]
    public class ProjectCommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectCommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{projectId:int}")]
        public async Task<IActionResult> GetComments(int projectId)
        {
            var comments = await _context.ProjectComments
                .Where(c => c.ProjectId == projectId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return Json(comments);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddComment([FromBody] ProjectComment model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.CreatedDate = DateTime.Now;

            await _context.ProjectComments.AddAsync(model);
            await _context.SaveChangesAsync();

            return Json(model);
        }
    }
}
