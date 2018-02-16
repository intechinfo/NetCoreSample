using System.Collections.Generic;
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
        readonly ClassGateway _classGateway;

        public ClassController( ClassGateway classGateway )
        {
            _classGateway = classGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetClassList()
        {
            IEnumerable<ClassData> result = await _classGateway.GetAll();
            return Ok( result );
        }

        [HttpGet( "{id}", Name = "GetClass" )]
        public async Task<IActionResult> GetClassById( int id )
        {
            DAL.Result<ClassData> result = await _classGateway.FindById2( id );
            return this.CreateResult( result );
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass( [FromBody] ClassViewModel model )
        {
            DAL.Result<int> result = await _classGateway.Create( model.Name, model.Level );
            return this.CreateResult( result, o =>
            {
                o.RouteName = "GetClass";
                o.RouteValues = id => new { id };
            } );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> UpdateClass( int id, [FromBody] ClassViewModel model )
        {
            DAL.Result result = await _classGateway.Update( id, model.Name, model.Level );
            return this.CreateResult( result );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteClass( int id )
        {
            DAL.Result result = await _classGateway.Delete( id );
            return this.CreateResult( result );
        }

        [HttpGet( "NotAssigned" )]
        public async Task<IActionResult> GetNotAssigned()
        {
            IEnumerable<ClassData> result = await _classGateway.GetNotAssigned();
            return this.Ok( result );
        }
    }
}