using System.Security.Claims;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly TokenService _tokenService;

        public HomeController( TokenService tokenService )
        {
            _tokenService = tokenService;
        }

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

        [Authorize( ActiveAuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        public IActionResult SinglePageApp()
        {
            string userId = User.FindFirst( ClaimTypes.NameIdentifier ).Value;
            string email = User.FindFirst( ClaimTypes.Email ).Value;
            Token token = _tokenService.GenerateToken( userId, email );
            ViewData[ "Token" ] = token;
            ViewData[ "Email" ] = email;
            ViewData[ "NoLayout" ] = true;
            return View();
        }
    }
}
