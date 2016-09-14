using System;
using System.Collections.Generic;
using ITI.PrimarySchool.DAL;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class StudentService
    {
        readonly StudentGateway _studentGateway;

        public StudentService( StudentGateway studentGateway )
        {
            _studentGateway = studentGateway;
        }

        public Result<Student> CreateStudent( string firstName, string lastName, DateTime birthDate )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Student>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Student>( Status.BadRequest, "The last name is not valid." );
            if( _studentGateway.FindByName( firstName, lastName ) != null ) return Result.Failure<Student>( Status.BadRequest, "A student with this name already exists." );

            _studentGateway.Create( firstName, lastName, birthDate );
            Student student = _studentGateway.FindByName( firstName, lastName );
            return Result.Success( Status.Created, student );
        }

        public Result<Student> UpdateStudent( int studentId, string firstName, string lastName, DateTime birthDate )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Student>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Student>( Status.BadRequest, "The last name is not valid." );
            if( _studentGateway.FindByName( firstName, lastName ) != null ) return Result.Failure<Student>( Status.BadRequest, "A student with this name already exists." );

            _studentGateway.Update( studentId, firstName, lastName, birthDate );
            Student student = _studentGateway.FindById( studentId );
            return Result.Success( Status.Ok, student );
        }

        public Result<Student> GetById( int id )
        {
            Student student;
            if( ( student = _studentGateway.FindById( id ) ) == null ) return Result.Failure<Student>( Status.NotFound, "Student not found." );
            return Result.Success( Status.Ok, student );
        }

        public Result<int> Delete( int classId )
        {
            if( _studentGateway.FindById( classId ) == null ) return Result.Failure<int>( Status.NotFound, "Student not found." );
            _studentGateway.Delete( classId );
            return Result.Success( Status.Ok, classId );
        }

        public Result<IEnumerable<Student>> GetAll()
        {
            return Result.Success( Status.Ok, _studentGateway.GetAll() );
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
