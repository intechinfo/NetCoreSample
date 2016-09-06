using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public class GoogleExternalAuthenticationManager : IExternalAuthenticationManager
    {
        readonly UserService _userService;

        public GoogleExternalAuthenticationManager( UserService userService )
        {
            _userService = userService;
        }

        public void CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            _userService.CreateOrUpdateGoogleUser( context.GetEmail(), context.RefreshToken );
        }

        public User FindUser( OAuthCreatingTicketContext context )
        {
            return _userService.FindUser( context.GetEmail() );
        }
    }
}
