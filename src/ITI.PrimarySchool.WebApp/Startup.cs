using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.AddMvc();
            services.AddTransient( _ => new UserGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddTransient<PasswordHasher>();
            services.AddTransient<UserService>();
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddConsole();

            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookieAuthentication( new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieAuthentication.AuthenticationScheme
            } );

            GithubAuthenticationEvents githubAuthenticationEvents = new GithubAuthenticationEvents( app.ApplicationServices.GetRequiredService<UserService>() );

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

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" } );
            } );

            app.UseStaticFiles();
        }
    }
}
