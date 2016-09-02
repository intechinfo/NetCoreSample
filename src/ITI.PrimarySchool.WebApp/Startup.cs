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

            GoogleAuthenticationEvents googleAuthenticationEvents = new GoogleAuthenticationEvents( app.ApplicationServices.GetRequiredService<UserService>() );

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
            } );

            app.UseStaticFiles();
        }
    }
}
