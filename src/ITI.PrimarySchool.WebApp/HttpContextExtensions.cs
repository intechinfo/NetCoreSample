/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace Mvc.Client.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<bool> IsProviderSupported( this IAuthenticationSchemeProvider authSchemeProvider, string provider )
        {
            if(authSchemeProvider == null ) throw new ArgumentNullException( nameof(authSchemeProvider) );

            return ( from schemes in await authSchemeProvider.GetAllSchemesAsync()
                     where string.Equals(schemes.Name, provider, StringComparison.OrdinalIgnoreCase )
                     select schemes.Name).Any();
        }
    }
}