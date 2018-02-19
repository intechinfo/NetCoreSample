using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public class GithubAuthenticationManager : AuthenticationManager<GithubUserInfo>
    {
        readonly UserGateway _userGateway;

        public GithubAuthenticationManager( UserService userService, UserGateway userGateway )
        {
            _userGateway = userGateway;
        }

        protected override Task CreateOrUpdateUser( GithubUserInfo userInfo )
        {
            return _userGateway.CreateOrUpdateGithubUser(
                userInfo.Email,
                userInfo.GithubId,
                userInfo.AccessToken );
        }

        protected override Task<UserData> FindUser( GithubUserInfo userInfo )
        {
            return _userGateway.FindByGithubId( userInfo.GithubId );
        }

        protected override async Task<GithubUserInfo> GetUserInfoFromContext( OAuthCreatingTicketContext ctx )
        {
            using( var request = new HttpRequestMessage( HttpMethod.Get, ctx.Options.UserInformationEndpoint ) )
            {
                request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
                request.Headers.Authorization = new AuthenticationHeaderValue( "Bearer", ctx.AccessToken );

                using( var response = await ctx.Backchannel.SendAsync( request, HttpCompletionOption.ResponseHeadersRead, ctx.HttpContext.RequestAborted ) )
                {
                    response.EnsureSuccessStatusCode();
                    JObject githubUser = JObject.Parse( await response.Content.ReadAsStringAsync() );
                    return new GithubUserInfo
                    {
                        AccessToken = ctx.AccessToken,
                        Email = githubUser["email"].Value<string>(),
                        GithubId = githubUser["id"].Value<int>()
                    };
                }
            }
        }
    }

    public class GithubUserInfo
    {
        public string Email { get; set; }

        public int GithubId { get; set; }

        public string AccessToken { get; set; }
    }
}
