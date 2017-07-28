using System.Text;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;

namespace ITI.PrimarySchool.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            string secretKey = Configuration["JwtBearer:SigningKey"];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            services.AddAuthentication()
                .AddCookieAuthentication(CookieAuthentication.AuthenticationScheme)

                .AddGoogleAuthentication(o =>
                {
                    o.SignInScheme = CookieAuthentication.AuthenticationScheme;
                    o.ClientId = Configuration["Authentication:Github:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Github:ClientSecret"];
                    o.Scope.Add("user");
                    o.Scope.Add("user:email");
                })

                .AddJwtBearerAuthentication(JwtBearerAuthentication.AuthenticationScheme, o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,

                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JwtBearer:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration["JwtBearer:Audience"],

                        NameClaimType = ClaimTypes.Email,
                        AuthenticationType = JwtBearerAuthentication.AuthenticationType
                    };
                });

            services.Configure<TokenProviderOptions>(o =>
            {
                o.Audience = Configuration["JwtBearer:Audience"];
                o.Issuer = Configuration["JwtBearer:Issuer"];
                o.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddMvc();
            services.AddSingleton(_ => new UserGateway(Configuration["ConnectionStrings:PrimarySchoolDB"]));
            services.AddSingleton(_ => new ClassGateway(Configuration["ConnectionStrings:PrimarySchoolDB"]));
            services.AddSingleton(_ => new StudentGateway(Configuration["ConnectionStrings:PrimarySchoolDB"]));
            services.AddSingleton(_ => new TeacherGateway(Configuration["ConnectionStrings:PrimarySchoolDB"]));
            services.AddSingleton<PasswordHasher>();
            services.AddSingleton<UserService>();
            services.AddSingleton<TokenService>();
            services.AddSingleton<ClassService>();
            services.AddSingleton<StudentService>();
            services.AddSingleton<TeacherService>();
            services.AddSingleton<GitHubService>();
            services.AddSingleton<GitHubClient>();
            services.AddSingleton<IClaimsTransformation, ClaimsTransformer>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            
                routes.MapRoute(
                    name: "spa-fallback",
                    template: "Home/{*anything}",
                    defaults: new { controller = "Home", action = "Index" });
            });

            app.UseStaticFiles();
        }
    }
}
