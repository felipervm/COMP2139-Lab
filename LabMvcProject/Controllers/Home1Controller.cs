using Microsoft.AspNetCore.Mvc;

namespace LabMvcProject.Controllers
{
    public class Home1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
