using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Models.TeacherViewModels;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class TeacherController : Controller
    {
        readonly TeacherService _teacherService;

        public TeacherController( TeacherService teacherService )
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherList()
        {
            Services.Result<IEnumerable<Teacher>> result = await _teacherService.GetAll();
            return this.CreateResult<IEnumerable<Teacher>, IEnumerable<TeacherViewModel>>( result, o =>
            {
                o.ToViewModel = x => x.Select( t => t.ToTeacherViewModel() );
            } );
        }

        [HttpGet( "{id}", Name = "GetTeacher" )]
        public async Task<IActionResult> GetTeacherById( int id )
        {
            Services.Result<Teacher> result = await _teacherService.GetById( id );
            return this.CreateResult<Teacher, TeacherViewModel>( result, o =>
            {
                o.ToViewModel = t => t.ToTeacherViewModel();
            } );
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher( [FromBody] TeacherViewModel model )
        {
            Services.Result<Teacher> result = await _teacherService.CreateTeacher( model.FirstName, model.LastName );
            return this.CreateResult<Teacher, TeacherViewModel>( result, o =>
            {
                o.ToViewModel = t => t.ToTeacherViewModel();
                o.RouteName = "GetTeacher";
                o.RouteValues = t => new { id = t.TeacherId };
            } );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> UpdateTeacher( int id, [FromBody] TeacherViewModel model )
        {
            Services.Result<Teacher> result = await _teacherService.UpdateTeacher( id, model.FirstName, model.LastName );
            return this.CreateResult<Teacher, TeacherViewModel>( result, o =>
            {
                o.ToViewModel = t => t.ToTeacherViewModel();
            } );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteTeacher( int id )
        {
            Services.Result<int> result = await _teacherService.Delete( id );
            return this.CreateResult( result );
        }

        [HttpPost( "{id}/assignClass" )]
        public async Task<IActionResult> AssignClass( int id, [FromBody] AssignClassViewModel model )
        {
            Services.Result result = await _teacherService.AssignClass( id, model.ClassId );
            return this.CreateResult( result );
        }

        [HttpGet( "{id}/assignedClass" )]
        public async Task<IActionResult> AssignedClass( int id )
        {
            Services.Result<Class> result = await _teacherService.AssignedClass( id );
            return this.CreateResult<Class, AssignedClassViewModel>( result, o =>
            {
                o.ToViewModel = c => c.ToAssignedClassViewModel();
            } );
        }
    }
}