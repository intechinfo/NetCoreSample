using System;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public class TokenProviderMiddleware
    {
        readonly RequestDelegate _next;
        readonly TokenProviderOptions _options;
        readonly UserService _userService;
        readonly TokenService _tokenService;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            UserService userService,
            TokenService tokenService )
        {
            _next = next;
            _options = options.Value;
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task Invoke( HttpContext context )
        {
            // If the request path doesn't match, skip
            if( !context.Request.Path.Equals( _options.Path, StringComparison.Ordinal ) )
            {
                await _next( context );
                return;
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if( !context.Request.Method.Equals( "POST" )
               || !context.Request.HasFormContentType )
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync( "Bad request." );
                return;
            }

            var username = context.Request.Form[ "username" ];
            var password = context.Request.Form[ "password" ];
            User user = _userService.FindUser( username, password );
            if( user == null )
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync( "Invalid username or password." );
                return;
            }

            Token token = _tokenService.GenerateToken( user.UserId.ToString(), username );
            var response = new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn
            };

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync( JsonConvert.SerializeObject( response, new JsonSerializerSettings { Formatting = Formatting.Indented } ) );
        }
    }
}