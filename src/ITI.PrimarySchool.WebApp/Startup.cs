using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

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

                .AddGoogleAuthentication(c =>
                {
                    c.SignInScheme = CookieAuthentication.AuthenticationScheme;
                    c.ClientId = Configuration["Authentication:Google:ClientId"];
                    c.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    c.AccessType = "offline";
                    c.Events = new OAuthEvents
                    {
                        OnCreatingTicket = ctx =>
                        {
                            ExternalAuthenticationEvents googleAuthenticationEvents = new ExternalAuthenticationEvents(
                                new GoogleExternalAuthenticationManager(ctx.HttpContext.RequestServices.GetRequiredService<UserService>()));
                            return googleAuthenticationEvents.OnCreatingTicket(ctx);
                        }
                    };
                })
                
                .AddOAuthAuthentication("GitHub", o =>
                {
                    o.SignInScheme = CookieAuthentication.AuthenticationScheme;
                    o.ClientId = Configuration["Authentication:Github:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Github:ClientSecret"];
                    o.CallbackPath = new PathString("/signin-github");
                    o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                    o.TokenEndpoint = "https://github.com/login/oauth/access_token";
                    o.UserInformationEndpoint = "https://api.github.com/user";
                    o.Scope.Add("user");
                    o.Scope.Add("user:email");
                    o.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
                    o.ClaimActions.MapJsonKey("urn:github:name", "name");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
                    o.ClaimActions.MapJsonKey("urn:github:url", "url");
                    o.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async ctx =>
                        {
                            // Get the GitHub user
                            var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var response = await ctx.Backchannel.SendAsync(request, ctx.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                            ctx.RunClaimActions(user);

                            ExternalAuthenticationEvents githubAuthenticationEvents = new ExternalAuthenticationEvents(
                                new GithubExternalAuthenticationManager(ctx.HttpContext.RequestServices.GetRequiredService<UserService>()));
                            await githubAuthenticationEvents.OnCreatingTicket(ctx);
                        }
                    };
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
