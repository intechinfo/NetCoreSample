using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<Result<Student>> CreateStudent( string firstName, string lastName, DateTime birthDate, string gitHubLogin )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Student>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Student>( Status.BadRequest, "The last name is not valid." );
            if( await _studentGateway.FindByName( firstName, lastName ) != null ) return Result.Failure<Student>( Status.BadRequest, "A student with this name already exists." );
            if( !string.IsNullOrEmpty( gitHubLogin ) && await _studentGateway.FindByGitHubLogin( gitHubLogin ) != null ) return Result.Failure<Student>( Status.BadRequest, "A student with GitHub login already exists." );
            await _studentGateway.Create( firstName, lastName, birthDate, gitHubLogin );
            Student student = await _studentGateway.FindByName( firstName, lastName );
            return Result.Success( Status.Created, student );
        }

        public async Task<Result<Student>> UpdateStudent( int studentId, string firstName, string lastName, DateTime birthDate, string gitHubLogin )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Student>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Student>( Status.BadRequest, "The last name is not valid." );
            Student student;
            if( ( student = await _studentGateway.FindById( studentId ) ) == null )
            {
                return Result.Failure<Student>( Status.NotFound, "Student not found." );
            }

            {
                Student s = await _studentGateway.FindByName( firstName, lastName );
                if( s != null && s.StudentId != student.StudentId ) return Result.Failure<Student>( Status.BadRequest, "A student with this name already exists." );
            }

            if( !string.IsNullOrEmpty( gitHubLogin ) )
            {
                Student s = await _studentGateway.FindByGitHubLogin( gitHubLogin );
                if( s != null && s.StudentId != student.StudentId ) return Result.Failure<Student>( Status.BadRequest, "A student with this GitHub login already exists." );
            }

            await _studentGateway.Update( studentId, firstName, lastName, birthDate, gitHubLogin );
            student = await _studentGateway.FindById( studentId );
            return Result.Success( Status.Ok, student );
        }

        public async Task<Result<Student>> GetById( int id )
        {
            Student student;
            if( ( student = await _studentGateway.FindById( id ) ) == null ) return Result.Failure<Student>( Status.NotFound, "Student not found." );
            return Result.Success( Status.Ok, student );
        }

        public async Task<Result<int>> Delete( int classId )
        {
            if( await _studentGateway.FindById( classId ) == null ) return Result.Failure<int>( Status.NotFound, "Student not found." );
            await _studentGateway.Delete( classId );
            return Result.Success( Status.Ok, classId );
        }

        public async Task<Result<IEnumerable<Student>>> GetAll()
        {
            return Result.Success( Status.Ok, await _studentGateway.GetAll() );
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
