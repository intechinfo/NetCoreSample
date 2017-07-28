using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        readonly IHttpContextAccessor _httpAccessor;

        public ClaimsTransformer(IHttpContextAccessor httpAccessor)
        {
            if (httpAccessor == null) throw new ArgumentNullException(nameof(httpAccessor));
            _httpAccessor = httpAccessor;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            return Task.FromResult(principal);
        }
    }
}
