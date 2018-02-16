using System.Collections.Generic;
using System.Linq;
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
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class StudentController : Controller
    {
        readonly StudentService _studentService;

        public StudentController( StudentService studentService )
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentList()
        {
            Services.Result<IEnumerable<Student>> result = await _studentService.GetAll();
            return this.CreateResult<IEnumerable<Student>, IEnumerable<StudentViewModel>>( result, o =>
            {
                o.ToViewModel = x => x.Select( s => s.ToStudentViewModel() );
            } );
        }

        [HttpGet( "{id}", Name = "GetStudent" )]
        public async Task<IActionResult> GetStudentById( int id )
        {
            Services.Result<Student> result = await _studentService.GetById( id );
            return this.CreateResult<Student, StudentViewModel>( result, o =>
            {
                o.ToViewModel = s => s.ToStudentViewModel();
            } );
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent( [FromBody] StudentViewModel model )
        {
            Services.Result<Student> result = await _studentService.CreateStudent( model.FirstName, model.LastName, model.BirthDate, model.GitHubLogin );
            return this.CreateResult<Student, StudentViewModel>( result, o =>
            {
                o.ToViewModel = s => s.ToStudentViewModel();
                o.RouteName = "GetStudent";
                o.RouteValues = s => new { id = s.StudentId };
            } );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> UpdateStudent( int id, [FromBody] StudentViewModel model )
        {
            Services.Result<Student> result = await _studentService.UpdateStudent( id, model.FirstName, model.LastName, model.BirthDate, model.GitHubLogin );
            return this.CreateResult<Student, StudentViewModel>( result, o =>
            {
                o.ToViewModel = s => s.ToStudentViewModel();
            } );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteStudent( int id )
        {
            Services.Result<int> result = await _studentService.Delete( id );
            return this.CreateResult( result );
        }
    }
}