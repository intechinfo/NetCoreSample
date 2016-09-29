using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Models.StudentViewModels;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class GitHubController : Controller
    {
        readonly GitHubService _gitHubService;

        public GitHubController( GitHubService gitHubService )
        {
            _gitHubService = gitHubService;
        }

        [HttpGet( "Following" )]
        public async Task<IActionResult> GetFollowedStudents()
        {
            int userId = int.Parse( User.FindFirst( ClaimTypes.NameIdentifier ).Value );
            Result<IEnumerable<Student>> result = await _gitHubService.GetFollowedStudents( userId );
            return this.CreateResult<IEnumerable<Student>, IEnumerable<FollowedStudentViewModel>>( result, o =>
            {
                o.ToViewModel = x => x.Select( s => s.ToFollowedStudentViewModel() );
            } );
        }
    }
}
