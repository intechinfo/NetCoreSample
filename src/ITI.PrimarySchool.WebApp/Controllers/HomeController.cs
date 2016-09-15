using System.Linq;
using System.Security.Claims;
using ITI.PrimarySchool.WebApp.Services;
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
            ClaimsIdentity identity = User.Identities.FirstOrDefault( i => i.AuthenticationType == "Cookies" );
            if( identity != null )
            {
                string userId = identity.FindFirst( ClaimTypes.NameIdentifier ).Value;
                string email = identity.FindFirst( ClaimTypes.Email ).Value;
                Token token = _tokenService.GenerateToken( userId, email );
                ViewData[ "Token" ] = token;
                ViewData[ "Email" ] = email;
            }
            else
            {
                ViewData[ "Token" ] = null;
                ViewData[ "Email" ] = null;
            }
            
            ViewData[ "NoLayout" ] = true;
            return View();
        }
    }
}
