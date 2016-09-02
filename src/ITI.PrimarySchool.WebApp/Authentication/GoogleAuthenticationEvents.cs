using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public class GoogleAuthenticationEvents
    {
        readonly UserService _userService;

        public GoogleAuthenticationEvents( UserService userService )
        {
            _userService = userService;
        }

        public Task OnCreatingTicket( OAuthCreatingTicketContext context )
        {
            string email = GetEmail( context );
            _userService.CreateOrUpdateGoogleUser( email, context.RefreshToken );
            User user = _userService.FindUser( email );
            ClaimsPrincipal principal = CreatePrincipal( user );
            context.Ticket = new AuthenticationTicket( principal, context.Ticket.Properties, CookieAuthentication.AuthenticationScheme );
            return Task.CompletedTask;
        }

        ClaimsPrincipal CreatePrincipal( User user )
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim( ClaimTypes.NameIdentifier, user.UserId.ToString(), ClaimValueTypes.String ),
                new Claim( ClaimTypes.Email, user.Email )
            };
            ClaimsPrincipal principal = new ClaimsPrincipal( new ClaimsIdentity( claims, "Cookies", ClaimTypes.Email, string.Empty ) );
            return principal;
        }

        string GetEmail( OAuthCreatingTicketContext context )
        {
            return context.Identity.FindFirst( c => c.Type == ClaimTypes.Email ).Value;
        }
    }
}
