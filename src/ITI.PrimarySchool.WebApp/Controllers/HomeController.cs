using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly TokenService _tokenService;
        readonly UserService _userService;

        public HomeController( TokenService tokenService, UserService userService )
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ClaimsIdentity identity = User.Identities.SingleOrDefault( i => i.AuthenticationType == CookieAuthentication.AuthenticationType );
            if( identity != null )
            {
                string userId = identity.FindFirst( ClaimTypes.NameIdentifier ).Value;
                string email = identity.FindFirst( ClaimTypes.Email ).Value;
                Token token = _tokenService.GenerateToken( userId, email );
                IEnumerable<string> providers = _userService.GetAuthenticationProviders( userId );
                ViewData[ "Token" ] = token;
                ViewData[ "Email" ] = email;
                ViewData[ "Providers" ] = providers;
            }
            else
            {
                ViewData[ "Token" ] = null;
                ViewData[ "Email" ] = null;
                ViewData[ "Providers" ] = null;
            }

            ViewData[ "NoLayout" ] = true;
            return View();
        }
    }
}
