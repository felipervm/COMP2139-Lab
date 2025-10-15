using Microsoft.AspNetCore.Mvc;
using LabMvcProject.Models;
using System.Linq;
using LabMvcProject.Data;

namespace LabMvcProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult NotFound()
        {
        return View();
        }


        [HttpGet]
        public IActionResult Search(string searchTerm, string searchType)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                ViewBag.Message = "Please enter a search term.";
                return View("SearchResults");
            }

            if (searchType == "Project")
            {
                var projects = _context.Projects
                    .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                    .ToList();

                ViewBag.SearchTerm = searchTerm;
                ViewBag.SearchType = "Project";
                return View("SearchResults", projects);
            }
            else if (searchType == "Task")
            {
                var tasks = _context.ProjectTasks
                    .Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm))
                    .ToList();

                ViewBag.SearchTerm = searchTerm;
                ViewBag.SearchType = "Task";
                return View("SearchResults", tasks);
            }

            ViewBag.Message = "Invalid search type selected.";
            return View("SearchResults");
        }

        [Route("Home/NotFound")]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}

