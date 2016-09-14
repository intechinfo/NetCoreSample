using System.Collections.Generic;
using ITI.PrimarySchool.DAL;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class TeacherService
    {
        readonly TeacherGateway _teacherGateway;

        public TeacherService( TeacherGateway teacherGateway )
        {
            _teacherGateway = teacherGateway;
        }

        public Result<Teacher> CreateTeacher( string firstName, string lastName )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Teacher>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Teacher>( Status.BadRequest, "The last name is not valid." );
            if( _teacherGateway.FindByName( firstName, lastName ) != null ) return Result.Failure<Teacher>( Status.BadRequest, "A teacher with this name already exists." );

            _teacherGateway.Create( firstName, lastName );
            Teacher teacher = _teacherGateway.FindByName( firstName, lastName );
            return Result.Success( Status.Created, teacher );
        }

        public Result<Teacher> UpdateTeacher( int teacherId, string firstName, string lastName )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Teacher>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Teacher>( Status.BadRequest, "The last name is not valid." );
            if( _teacherGateway.FindByName( firstName, lastName ) != null ) return Result.Failure<Teacher>( Status.BadRequest, "A teacher with this name already exists." );

            _teacherGateway.Update( teacherId, firstName, lastName );
            Teacher teacher = _teacherGateway.FindById( teacherId );
            return Result.Success( Status.Ok, teacher );
        }

        public Result<Teacher> GetById( int id )
        {
            Teacher teacher;
            if( ( teacher = _teacherGateway.FindById( id ) ) == null ) return Result.Failure<Teacher>( Status.NotFound, "Teacher not found." );
            return Result.Success( Status.Ok, teacher );
        }

        public Result<int> Delete( int teacherId )
        {
            if( _teacherGateway.FindById( teacherId ) == null ) return Result.Failure<int>( Status.NotFound, "Teacher not found." );
            _teacherGateway.Delete( teacherId );
            return Result.Success( Status.Ok, teacherId );
        }

        public Result<IEnumerable<Teacher>> GetAll()
        {
            return Result.Success( Status.Ok, _teacherGateway.GetAll() );
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
