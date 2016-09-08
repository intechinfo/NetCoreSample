using System.Text;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        public void ConfigureServices( IServiceCollection services )
        {
            services.AddOptions();

            string secretKey = Configuration[ "JwtBearer:SigningKey" ];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            services.Configure<TokenProviderOptions>( o =>
            {
                o.Audience = Configuration[ "JwtBearer:Audience" ];
                o.Issuer = Configuration[ "JwtBearer:Issuer" ];
                o.SigningCredentials = new SigningCredentials( signingKey, SecurityAlgorithms.HmacSha256 );
            } );

            services.AddMvc();
            services.AddTransient( _ => new UserGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddTransient<PasswordHasher>();
            services.AddTransient<UserService>();
            services.AddTransient<TokenService>();
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddConsole();

            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            string secretKey = Configuration[ "JwtBearer:SigningKey" ];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            app.UseMiddleware<TokenProviderMiddleware>();

            app.UseJwtBearerAuthentication( new JwtBearerOptions
            {
                AuthenticationScheme = JwtBearerAuthentication.AuthenticationScheme,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    ValidateIssuer = true,
                    ValidIssuer = Configuration[ "JwtBearer:Issuer" ],

                    ValidateAudience = true,
                    ValidAudience = Configuration[ "JwtBearer:Audience" ]
                }
            } );

            app.UseCookieAuthentication( new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieAuthentication.AuthenticationScheme
            } );

            ExternalAuthenticationEvents githubAuthenticationEvents = new ExternalAuthenticationEvents(
                new GithubExternalAuthenticationManager( app.ApplicationServices.GetRequiredService<UserService>() ) );
            ExternalAuthenticationEvents googleAuthenticationEvents = new ExternalAuthenticationEvents(
                new GoogleExternalAuthenticationManager( app.ApplicationServices.GetRequiredService<UserService>() ) );

            app.UseGitHubAuthentication( o =>
            {
                o.SignInScheme = CookieAuthentication.AuthenticationScheme;
                o.ClientId = Configuration[ "Authentication:Github:ClientId" ];
                o.ClientSecret = Configuration[ "Authentication:Github:ClientSecret" ];
                o.Scope.Add( "user" );
                o.Scope.Add( "user:email" );
                o.Events = new OAuthEvents
                {
                    OnCreatingTicket = githubAuthenticationEvents.OnCreatingTicket
                };
            } );

            app.UseGoogleAuthentication( c =>
            {
                c.SignInScheme = CookieAuthentication.AuthenticationScheme;
                c.ClientId = Configuration[ "Authentication:Google:ClientId" ];
                c.ClientSecret = Configuration[ "Authentication:Google:ClientSecret" ];
                c.Events = new OAuthEvents
                {
                    OnCreatingTicket = googleAuthenticationEvents.OnCreatingTicket
                };
                c.AccessType = "offline";
            } );

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" } );

                routes.MapRoute(
                    name: "spa-fallback",
                    template: "{controller}/{action}/{*anything}",
                    defaults: new { controller = "Home", action = "SinglePageApp" } );
            } );

            app.UseStaticFiles();
        }
    }
}
