using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public class GoogleAuthenticationManager : AuthenticationManager<GoogleUserInfo>
    {
        readonly UserGateway _userGateway;

        public GoogleAuthenticationManager( UserService userService, UserGateway userGateway )
        {
            _userGateway = userGateway;
        }

        protected override async Task CreateOrUpdateUser( GoogleUserInfo userInfo )
        {
            if( userInfo.RefreshToken != null )
            {
                await _userGateway.CreateOrUpdateGoogleUser( userInfo.Email, userInfo.GoogleId, userInfo.RefreshToken );
            }
        }

        protected override Task<UserData> FindUser( GoogleUserInfo userInfo )
        {
            return _userGateway.FindByGoogleId( userInfo.GoogleId );
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
