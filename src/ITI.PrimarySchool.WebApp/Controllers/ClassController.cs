using System.Collections.Generic;
using System.Linq;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Models.ClassViewModels;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class ClassController : Controller
    {
        readonly ClassService _classService;

        public ClassController( ClassService classService )
        {
            _classService = classService;
        }

        [HttpGet]
        public IActionResult GetClassList()
        {
            Result<IEnumerable<Class>> result = _classService.GetAll();
            return this.CreateResult<IEnumerable<Class>, IEnumerable<ClassViewModel>>( result, o =>
            {
                o.ToViewModel = x => x.Select( c => c.ToClassViewModel() );
            } );
        }

        [HttpGet( "{id}", Name = "GetClass" )]
        public IActionResult GetClassById( int id )
        {
            Result<Class> result = _classService.GetById( id );
            return this.CreateResult<Class, ClassViewModel>( result, o =>
            {
                o.ToViewModel = c => c.ToClassViewModel();
            } );
        }

        [HttpPost]
        public IActionResult CreateClass( [FromBody] ClassViewModel model )
        {
            Result<Class> result = _classService.CreateClass( model.Name, model.Level );
            return this.CreateResult<Class, ClassViewModel>( result, o =>
            {
                o.ToViewModel = c => c.ToClassViewModel();
                o.RouteName = "GetClass";
                o.RouteValues = c => new { id = c.ClassId };
            } );
        }

        [HttpPut( "{id}" )]
        public IActionResult UpdateClass( int id, [FromBody] ClassViewModel model )
        {
            Result<Class> result = _classService.UpdateClass( id, model.Name, model.Level );
            return this.CreateResult<Class, ClassViewModel>( result, o =>
            {
                o.ToViewModel = c => c.ToClassViewModel();
            } );
        }

        [HttpDelete( "{id}" )]
        public IActionResult DeleteClass( int id )
        {
            Result<int> result = _classService.Delete( id );
            return this.CreateResult( result );
        }

        [HttpGet( "NotAssigned" )]
        public IActionResult GetNotAssigned()
        {
            Result<IEnumerable<Class>> result = _classService.GetNotAssigned();
            return this.CreateResult<IEnumerable<Class>, IEnumerable<ClassViewModel>>( result, o =>
            {
                o.ToViewModel = x => x.Select( c => c.ToClassViewModel() );
            } );
        }
    }
}