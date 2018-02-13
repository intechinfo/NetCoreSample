using System.Collections.Generic;
using System.Linq;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Models.TeacherViewModels;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class TeacherController : Controller
    {
        readonly TeacherService _teacherService;

        public TeacherController( TeacherService teacherService )
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public IActionResult GetTeacherList()
        {
            Result<IEnumerable<Teacher>> result = _teacherService.GetAll();
            return this.CreateResult<IEnumerable<Teacher>, IEnumerable<TeacherViewModel>>( result, o =>
            {
                o.ToViewModel = x => x.Select( t => t.ToTeacherViewModel() );
            } );
        }

        [HttpGet( "{id}", Name = "GetTeacher" )]
        public IActionResult GetTeacherById( int id )
        {
            Result<Teacher> result = _teacherService.GetById( id );
            return this.CreateResult<Teacher, TeacherViewModel>( result, o =>
            {
                o.ToViewModel = t => t.ToTeacherViewModel();
            } );
        }

        [HttpPost]
        public IActionResult CreateTeacher( [FromBody] TeacherViewModel model )
        {
            Result<Teacher> result = _teacherService.CreateTeacher( model.FirstName, model.LastName );
            return this.CreateResult<Teacher, TeacherViewModel>( result, o =>
            {
                o.ToViewModel = t => t.ToTeacherViewModel();
                o.RouteName = "GetTeacher";
                o.RouteValues = t => new { id = t.TeacherId };
            } );
        }

        [HttpPut( "{id}" )]
        public IActionResult UpdateTeacher( int id, [FromBody] TeacherViewModel model )
        {
            Result<Teacher> result = _teacherService.UpdateTeacher( id, model.FirstName, model.LastName );
            return this.CreateResult<Teacher, TeacherViewModel>( result, o =>
            {
                o.ToViewModel = t => t.ToTeacherViewModel();
            } );
        }

        [HttpDelete( "{id}" )]
        public IActionResult DeleteTeacher( int id )
        {
            Result<int> result = _teacherService.Delete( id );
            return this.CreateResult( result );
        }

        [HttpPost( "{id}/assignClass" )]
        public IActionResult AssignClass( int id, [FromBody] AssignClassViewModel model )
        {
            Result result = _teacherService.AssignClass( id, model.ClassId );
            return this.CreateResult( result );
        }

        [HttpGet( "{id}/assignedClass" )]
        public IActionResult AssignedClass( int id )
        {
            Result<Class> result = _teacherService.AssignedClass( id );
            return this.CreateResult<Class, AssignedClassViewModel>( result, o =>
            {
                o.ToViewModel = c => c.ToAssignedClassViewModel();
            } );
        }
    }
}