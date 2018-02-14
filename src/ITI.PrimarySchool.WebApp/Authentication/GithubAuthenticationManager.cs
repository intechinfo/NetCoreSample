using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public class GithubAuthenticationManager
    {
        readonly UserService _userService;

        public GithubAuthenticationManager( UserService userService )
        {
            _userService = userService;
        }

        public async Task OnCreatingTicket( OAuthCreatingTicketContext ctx )
        {
            using( var request = new HttpRequestMessage( HttpMethod.Get, ctx.Options.UserInformationEndpoint ) )
            {
                request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
                request.Headers.Authorization = new AuthenticationHeaderValue( "Bearer", ctx.AccessToken );

                using( var response = await ctx.Backchannel.SendAsync( request, HttpCompletionOption.ResponseHeadersRead, ctx.HttpContext.RequestAborted ) )
                {
                    response.EnsureSuccessStatusCode();
                    JObject githubUser = JObject.Parse( await response.Content.ReadAsStringAsync() );

                    CreateOrUpdateUser( githubUser, ctx.AccessToken );
                    User user = FindUser( githubUser );
                    ClaimsPrincipal principal = CreatePrincipal( user );
                    ctx.Principal = principal;
                }
            }
        }

        public void CreateOrUpdateUser( JObject user, string accessToken )
        {
            _userService.CreateOrUpdateGithubUser(
                user[ "email" ].Value<string>(),
                user[ "id" ].Value<int>(),
                accessToken );
        }

        public User FindUser( JObject user )
        {
            return _userService.FindGithubUser( user[ "id" ].Value<int>() );
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
