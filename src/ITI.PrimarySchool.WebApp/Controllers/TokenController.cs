using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    [Route("/api/[controller]")]
    public class TokenController : Controller
    {
        readonly UserGateway _userGateway;
        readonly TokenService _tokenService;

        public TokenController( UserGateway userGateway, TokenService tokenService )
        {
            _userGateway = userGateway;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Token()
        {
            var identity = User.Identities.SingleOrDefault( i => i.AuthenticationType == CookieAuthentication.AuthenticationScheme );
            if( identity == null ) return Ok( new { Success = false } );

            string userId = identity.FindFirst( ClaimTypes.NameIdentifier ).Value;
            string email = identity.FindFirst( ClaimTypes.Email ).Value;
            Token token = _tokenService.GenerateToken( userId, email );
            IEnumerable<string> providers = await _userGateway.GetAuthenticationProviders( userId );
            return Ok( new { Success = true, Bearer = token, Email = email, BoundProviders = providers } );
        }
    }
}
