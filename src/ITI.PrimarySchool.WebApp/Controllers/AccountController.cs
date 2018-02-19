using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Models.AccountViewModels;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public class AccountController : Controller
    {
        readonly UserGateway _userGateway;
        readonly UserService _userService;
        readonly TokenService _tokenService;
        readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
        readonly Random _random;

        public AccountController( UserGateway userGateway, UserService userService, TokenService tokenService, IAuthenticationSchemeProvider authenticationSchemeProvider )
        {
            _userGateway = userGateway;
            _userService = userService;
            _tokenService = tokenService;
            _authenticationSchemeProvider = authenticationSchemeProvider;
            _random = new Random();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginViewModel model )
        {
            if( ModelState.IsValid )
            {
                UserData user = await _userService.FindUser( model.Email, model.Password );
                if( user == null )
                {
                    ModelState.AddModelError( string.Empty, "Invalid login attempt." );
                    return View( model );
                }
                await SignIn( user.Email, user.UserId.ToString() );
                return RedirectToAction( nameof( Authenticated ) );
            }

            return View( model );
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register( RegisterViewModel model )
        {
            if( ModelState.IsValid )
            {
                Result<int> result = await _userService.CreatePasswordUser( model.Email, model.Password );
                if( result.HasError )
                {
                    ModelState.AddModelError( string.Empty, result.ErrorMessage );
                    return View( model );
                }
                await SignIn( model.Email, result.Content.ToString() );
                return RedirectToAction( nameof( Authenticated ) );
            }

            return View( model );
        }

        [HttpGet]
        [Authorize( AuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync( CookieAuthentication.AuthenticationScheme );
            ViewData[ "NoLayout" ] = true;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin( [FromQuery] string provider )
        {
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.
            if( string.IsNullOrWhiteSpace( provider ) )
            {
                return BadRequest();
            }

            if( await _authenticationSchemeProvider.GetSchemeAsync( provider ) == null )
            {
                return BadRequest();
            }

            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
            string redirectUri = Url.Action( nameof( ExternalLoginCallback ), "Account" );
            return Challenge( new AuthenticationProperties { RedirectUri = redirectUri }, provider );
        }

        [HttpGet]
        [Authorize( AuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        public IActionResult ExternalLoginCallback()
        {
            return RedirectToAction( nameof( Authenticated ) );
        }

        [HttpGet]
        [Authorize( AuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        public async Task<IActionResult> Authenticated()
        {
            string userId = User.FindFirst( ClaimTypes.NameIdentifier ).Value;
            string email = User.FindFirst( ClaimTypes.Email ).Value;
            Token token = _tokenService.GenerateToken( userId, email );
            IEnumerable<string> providers = await _userGateway.GetAuthenticationProviders( userId );
            ViewData[ "BreachPadding" ] = GetBreachPadding(); // Mitigate BREACH attack. See http://www.breachattack.com/
            ViewData[ "Token" ] = token;
            ViewData[ "Email" ] = email;
            ViewData[ "NoLayout" ] = true;
            ViewData[ "Providers" ] = providers;
            return View();
        }

        async Task SignIn( string email, string userId )
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim( ClaimTypes.Email, email, ClaimValueTypes.String ),
                new Claim( ClaimTypes.NameIdentifier, userId.ToString(), ClaimValueTypes.String )
            };
            ClaimsIdentity identity = new ClaimsIdentity( claims, CookieAuthentication.AuthenticationType, ClaimTypes.Email, string.Empty );
            ClaimsPrincipal principal = new ClaimsPrincipal( identity );
            await HttpContext.SignInAsync( CookieAuthentication.AuthenticationScheme, principal );
        }

        string GetBreachPadding()
        {
            byte[] data = new byte[ _random.Next( 64, 256 ) ];
            _random.NextBytes( data );
            return Convert.ToBase64String( data );
        }
    }
}
