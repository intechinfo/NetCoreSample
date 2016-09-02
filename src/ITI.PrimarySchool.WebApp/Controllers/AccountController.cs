using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public class AccountController : Controller
    {
        readonly UserGateway _userGateway;
        readonly PasswordHasher _passwordHasher;

        public AccountController( UserGateway userGateway, PasswordHasher passwordHasher )
        {
            if( userGateway == null ) throw new ArgumentNullException( nameof( userGateway ) );
            if( passwordHasher == null ) throw new ArgumentNullException( nameof( passwordHasher ) );

            _userGateway = userGateway;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login( string returnUrl = null )
        {
            ViewData[ "ReturnUrl" ] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginViewModel model, string returnUrl = null )
        {
            if( ModelState.IsValid )
            {
                User user = _userGateway.FindByEmail( model.Email );
                if( user == null || _passwordHasher.VerifyHashedPassword(user.Password, model.Password ) != PasswordVerificationResult.Success )
                {
                    ModelState.AddModelError( string.Empty, "Invalid login attempt." );
                    ViewData[ "ReturnUrl" ] = returnUrl;
                    return View( model );
                }
                List<Claim> claims = new List<Claim>
                {
                    new Claim( ClaimTypes.Email, model.Email, ClaimValueTypes.String ),
                    new Claim( ClaimTypes.NameIdentifier, user.UserId.ToString(), ClaimValueTypes.String )
                };
                ClaimsIdentity identity = new ClaimsIdentity( claims, "Cookies", ClaimTypes.Email, string.Empty );
                ClaimsPrincipal principal = new ClaimsPrincipal( identity );
                await HttpContext.Authentication.SignInAsync( CookieAuthentication.AuthenticationScheme, principal );
                return RedirectToLocal( returnUrl );
            }

            return View( model );
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register( string returnUrl = null )
        {
            ViewData[ "ReturnUrl" ] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register( RegisterViewModel model, string returnUrl = null )
        {
            if( ModelState.IsValid )
            {
                if( _userGateway.FindByEmail( model.Email ) != null )
                {
                    ModelState.AddModelError( string.Empty, "An account with this email already exists." );
                    return View( model );
                }
                _userGateway.Create( model.Email, _passwordHasher.HashPassword( model.Password ) );
                return RedirectToLocal( returnUrl );
            }

            return View( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.Authentication.SignOutAsync( CookieAuthentication.AuthenticationScheme );
            return RedirectToAction( "Index", "Home" );
        }

        IActionResult RedirectToLocal( string returnUrl )
        {
            if( Url.IsLocalUrl( returnUrl ) )
            {
                return Redirect( returnUrl );
            }
            else
            {
                return RedirectToAction( nameof( HomeController.Index ), "Home" );
            }
        }
    }
}
