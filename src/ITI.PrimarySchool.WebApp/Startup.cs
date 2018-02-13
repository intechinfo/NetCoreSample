using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Services;
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddAuthentication()
                .AddCookie( CookieAuthentication.AuthenticationScheme );

            string secretKey = Configuration[ "JwtBearer:SigningKey" ];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            services.Configure<TokenProviderOptions>( o =>
            {
                o.Audience = Configuration[ "JwtBearer:Audience" ];
                o.Issuer = Configuration[ "JwtBearer:Issuer" ];
                o.SigningCredentials = new SigningCredentials( signingKey, SecurityAlgorithms.HmacSha256 );
            } );

            services.AddMvc();
            services.AddSingleton( _ => new UserGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddSingleton( _ => new ClassGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddSingleton( _ => new StudentGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddSingleton( _ => new TeacherGateway( Configuration[ "ConnectionStrings:PrimarySchoolDB" ] ) );
            services.AddSingleton<PasswordHasher>();
            services.AddSingleton<UserService>();
            services.AddSingleton<TokenService>();
            services.AddSingleton<ClassService>();
            services.AddSingleton<StudentService>();
            services.AddSingleton<TeacherService>();
            services.AddSingleton<GitHubService>();
            services.AddSingleton<GitHubClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            string secretKey = Configuration[ "JwtBearer:SigningKey" ];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

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
