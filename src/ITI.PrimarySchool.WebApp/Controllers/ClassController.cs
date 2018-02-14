using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Authentication;
using ITI.PrimarySchool.WebApp.Models.ClassViewModels;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class ClassController : Controller
    {
        readonly ClassService _classService;

        public ClassController( ClassService classService )
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClassList()
        {
            Result<IEnumerable<Class>> result = await _classService.GetAll();
            return this.CreateResult<IEnumerable<Class>, IEnumerable<ClassViewModel>>( result, o =>
            {
                o.ToViewModel = x => x.Select( c => c.ToClassViewModel() );
            } );
        }

        [HttpGet( "{id}", Name = "GetClass" )]
        public async Task<IActionResult> GetClassById( int id )
        {
            Result<Class> result = await _classService.GetById( id );
            return this.CreateResult<Class, ClassViewModel>( result, o =>
            {
                o.ToViewModel = c => c.ToClassViewModel();
            } );
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass( [FromBody] ClassViewModel model )
        {
            Result<Class> result = await _classService.CreateClass( model.Name, model.Level );
            return this.CreateResult<Class, ClassViewModel>( result, o =>
            {
                o.ToViewModel = c => c.ToClassViewModel();
                o.RouteName = "GetClass";
                o.RouteValues = c => new { id = c.ClassId };
            } );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> UpdateClass( int id, [FromBody] ClassViewModel model )
        {
            Result<Class> result = await _classService.UpdateClass( id, model.Name, model.Level );
            return this.CreateResult<Class, ClassViewModel>( result, o =>
            {
                o.ToViewModel = c => c.ToClassViewModel();
            } );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteClass( int id )
        {
            Result<int> result = await _classService.Delete( id );
            return this.CreateResult( result );
        }

        [HttpGet( "NotAssigned" )]
        public async Task<IActionResult> GetNotAssigned()
        {
            Result<IEnumerable<Class>> result = await _classService.GetNotAssigned();
            return this.CreateResult<IEnumerable<Class>, IEnumerable<ClassViewModel>>( result, o =>
            {
                o.ToViewModel = x => x.Select( c => c.ToClassViewModel() );
            } );
        }
    }
}