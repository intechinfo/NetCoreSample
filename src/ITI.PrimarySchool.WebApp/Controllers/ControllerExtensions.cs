using System;
using ITI.PrimarySchool.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public static class ControllerExtensions
    {
        public static IActionResult CreateResult<T>( this Controller @this, Result<T> result )
        {
            return CreateResult( @this, result, new ActionResultOptions<T>( @this ) );
        }

        public static IActionResult CreateResult<T>(
            this Controller @this,
            Result<T> result,
            Action<ActionResultOptions<T>> options )
        {
            ActionResultOptions<T> o = new ActionResultOptions<T>( @this );
            options( o );
            return @this.CreateResult( result, o );
        }

        public static IActionResult CreateResult<T>(
            this Controller @this,
            Result<T> result,
            ActionResultOptions<T> options )
        {
            object value;
            if( !result.HasError ) value = result.Content;
            else value = result.ErrorMessage;

            if( result.Status == Status.Ok ) return @this.Ok( value );
            if( result.Status == Status.NotFound ) return @this.NotFound( value );
            if( result.Status == Status.BadRequest ) return @this.BadRequest( value );
            if( result.Status == Status.Created )
            {
                return @this.CreatedAtRoute( options.RouteName, options.RouteValues( result.Content ), value );
            }

            throw new ArgumentException( "Unknown status.", nameof( result ) );
        }

        public static IActionResult CreateResult( this Controller @this, Result result )
        {
            if( result.Status == Status.Ok ) return @this.Ok();
            if( result.Status == Status.NotFound ) return @this.NotFound( result.ErrorMessage );
            if( result.Status == Status.BadRequest ) return @this.BadRequest( result.ErrorMessage );

            throw new ArgumentException( "Unknown status.", nameof( result ) );
        }
    }
}
