using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public class GoogleAuthenticationManager
    {
        readonly UserService _userService;

        public GoogleAuthenticationManager( UserService userService )
        {
            _userService = userService;
        }

        public Task OnCreatingTicket( OAuthCreatingTicketContext ctx )
        {
            CreateOrUpdateUser( ctx );
            User user = FindUser( ctx );
            ClaimsPrincipal principal = CreatePrincipal( user );
            ctx.Principal = principal;
            return Task.CompletedTask;
        }

        public void CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            if( context.RefreshToken != null )
            {
                _userService.CreateOrUpdateGoogleUser( context.GetEmail(), context.GetGoogleId(), context.RefreshToken );
            }
        }

        public User FindUser( OAuthCreatingTicketContext context )
        {
            return _userService.FindGoogleUser( context.GetGoogleId() );
        }

        ClaimsPrincipal CreatePrincipal( User user )
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim( ClaimTypes.NameIdentifier, user.UserId.ToString(), ClaimValueTypes.String ),
                new Claim( ClaimTypes.Email, user.Email )
            };
            ClaimsPrincipal principal = new ClaimsPrincipal( new ClaimsIdentity( claims, CookieAuthentication.AuthenticationType, ClaimTypes.Email, string.Empty ) );
            return principal;
        }
    }
}
