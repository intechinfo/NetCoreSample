using ITI.PrimarySchool.WebApp.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize( ActiveAuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        public IActionResult VerySecure()
        {
            return View();
        }
    }
}
