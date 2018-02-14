using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class ClassService
    {
        readonly ClassGateway _classGateway;

        public ClassService( ClassGateway classGateway )
        {
            _classGateway = classGateway;
        }

        public async Task<Result<Class>> CreateClass( string name, string level )
        {
            if( !IsNameValid( name ) ) return Result.Failure<Class>( Status.BadRequest, "The class name is not valid." );
            if( !IsLevelValid( level ) ) return Result.Failure<Class>( Status.BadRequest, "The class level is not valid." );
            if( await _classGateway.FindByName( name ) != null ) return Result.Failure<Class>( Status.BadRequest, "A class with this name already exists." );

            await _classGateway.Create( name, level );
            Class c = await _classGateway.FindByName( name );
            return Result.Success( Status.Created, c );
        }

        public async Task<Result<Class>> UpdateClass( int classId, string name, string level )
        {
            if( !IsNameValid( name ) ) return Result.Failure<Class>( Status.BadRequest, "The class name is not valid." );
            if( !IsLevelValid( level ) ) return Result.Failure<Class>( Status.BadRequest, "The class level is not valid." );
            Class c;
            if( ( c = await _classGateway.FindById( classId ) ) == null )
            {
                return Result.Failure<Class>( Status.NotFound, "Class not found." );
            }

            {
                Class other = await _classGateway.FindByName( name );
                if(other != null && other.ClassId != c.ClassId) return Result.Failure<Class>( Status.BadRequest, "A class with this name already exists." );
            }

            await _classGateway.Update( classId, name, level );
            c = await _classGateway.FindByName( name );
            return Result.Success( Status.Ok, c );
        }

        public async Task<Result<Class>> GetById( int id )
        {
            Class c;
            if( ( c = await _classGateway.FindById( id ) ) == null ) return Result.Failure<Class>( Status.NotFound, "Class not found." );
            return Result.Success( Status.Ok, c );
        }

        public async Task<Result<int>> Delete( int classId )
        {
            if( await _classGateway.FindById( classId ) == null ) return Result.Failure<int>( Status.NotFound, "Class not found." );
            await _classGateway.Delete( classId );
            return Result.Success( Status.Ok, classId );
        }

        public async Task<Result<IEnumerable<Class>>> GetAll()
        {
            return Result.Success( Status.Ok, await _classGateway.GetAll() );
        }

        public async Task<Result<IEnumerable<Class>>> GetNotAssigned()
        {
            return Result.Success( Status.Ok, await _classGateway.GetNotAssigned() );
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );

        bool IsLevelValid( string level ) =>
            level == "CP"
            || level == "CE1"
            || level == "CE2"
            || level == "CM1"
            || level == "CM2";
    }
}
