using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
