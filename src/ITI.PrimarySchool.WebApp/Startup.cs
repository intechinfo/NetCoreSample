using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ITI.PrimarySchool.WebApp
{
    public class Startup
    {
        public Startup( IHostingEnvironment env )
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath( env.ContentRootPath )
                .AddJsonFile( "appsettings.json", optional: true )
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddOptions();

            services.AddMvc();
            services.AddSingleton( _ => new UserGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddSingleton( _ => new ClassGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddSingleton( _ => new StudentGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddSingleton( _ => new TeacherGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddSingleton<PasswordHasher>();
            services.AddSingleton<UserService>();
            services.AddSingleton<TokenService>();
            services.AddSingleton<GitHubService>();
            services.AddSingleton<GitHubClient>();
            services.AddSingleton<GoogleAuthenticationManager>();
            services.AddSingleton<GithubAuthenticationManager>();

            string secretKey = Configuration[ "JwtBearer:SigningKey" ];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            services.Configure<TokenProviderOptions>( o =>
            {
                o.Audience = Configuration[ "JwtBearer:Audience" ];
                o.Issuer = Configuration[ "JwtBearer:Issuer" ];
                o.SigningCredentials = new SigningCredentials( signingKey, SecurityAlgorithms.HmacSha256 );
            } );

            services.AddAuthentication( CookieAuthentication.AuthenticationScheme )
                .AddCookie( CookieAuthentication.AuthenticationScheme )
                .AddJwtBearer( JwtBearerAuthentication.AuthenticationScheme,
                o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,

                        ValidateIssuer = true,
                        ValidIssuer = Configuration[ "JwtBearer:Issuer" ],

                        ValidateAudience = true,
                        ValidAudience = Configuration[ "JwtBearer:Audience" ],

                        NameClaimType = ClaimTypes.Email,
                        AuthenticationType = JwtBearerAuthentication.AuthenticationType
                    };
                } )
                .AddGoogle( o =>
                {
                    o.SignInScheme = CookieAuthentication.AuthenticationScheme;
                    o.ClientId = Configuration[ "Authentication:Google:ClientId" ];
                    o.ClientSecret = Configuration[ "Authentication:Google:ClientSecret" ];
                    o.Events = new OAuthEvents
                    {
                        OnCreatingTicket = ctx => ctx.HttpContext.RequestServices.GetRequiredService<GoogleAuthenticationManager>().OnCreatingTicket( ctx )
                    };
                    o.AccessType = "offline";
                } )
                .AddOAuth( "GitHub", o =>
                {
                    o.SignInScheme = CookieAuthentication.AuthenticationScheme;
                    o.ClientId = Configuration[ "Authentication:Github:ClientId" ];
                    o.ClientSecret = Configuration[ "Authentication:Github:ClientSecret" ];
                    o.CallbackPath = new PathString( "/signin-github" );

                    o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                    o.TokenEndpoint = "https://github.com/login/oauth/access_token";
                    o.UserInformationEndpoint = "https://api.github.com/user";

                    o.Scope.Add( "user" );

                    o.Events = new OAuthEvents
                    {
                        OnCreatingTicket = ctx => ctx.HttpContext.RequestServices.GetRequiredService<GithubAuthenticationManager>().OnCreatingTicket( ctx )
                    };
                } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            string secretKey = Configuration[ "JwtBearer:SigningKey" ];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            app.UseAuthentication();

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" } );

                routes.MapRoute(
                    name: "spa-fallback",
                    template: "Home/{*anything}",
                    defaults: new { controller = "Home", action = "Index" } );
            } );

            app.UseStaticFiles();
        }
    }
}
