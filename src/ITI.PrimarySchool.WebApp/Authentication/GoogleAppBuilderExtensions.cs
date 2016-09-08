using System;
using Microsoft.AspNetCore.Builder;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public static class GoogleAppBuilderExtensions
    {
        public static IApplicationBuilder UseGoogleAuthentication( this IApplicationBuilder app, Action<GoogleOptions> configuration )
        {
            GoogleOptions options = new GoogleOptions();
            configuration( options );
            app.UseGoogleAuthentication( options );
            return app;
        }
    }
}
