using System;
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
