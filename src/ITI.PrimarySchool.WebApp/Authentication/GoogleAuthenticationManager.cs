using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public class GoogleAuthenticationManager : AuthenticationManager<GoogleUserInfo>
    {
        readonly UserService _userService;

        public GoogleAuthenticationManager( UserService userService )
        {
            _userService = userService;
        }

        protected override async Task CreateOrUpdateUser( GoogleUserInfo userInfo )
        {
            if( userInfo.RefreshToken != null )
            {
                await _userService.CreateOrUpdateGoogleUser( userInfo.Email, userInfo.GoogleId, userInfo.RefreshToken );
            }
        }

        protected override Task<User> FindUser( GoogleUserInfo userInfo )
        {
            return _userService.FindGoogleUser( userInfo.GoogleId );
        }

        protected override Task<GoogleUserInfo> GetUserInfoFromContext( OAuthCreatingTicketContext ctx )
        {
            return Task.FromResult( new GoogleUserInfo
            {
                RefreshToken = ctx.RefreshToken,
                Email = ctx.GetEmail(),
                GoogleId = ctx.GetGoogleId()
            } );
        }
    }

    public class GoogleUserInfo
    {
        public string RefreshToken { get; set; }

        public string Email { get; set; }

        public string GoogleId { get; set; }
    }
}
