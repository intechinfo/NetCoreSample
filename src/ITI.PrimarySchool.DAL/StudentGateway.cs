using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace ITI.PrimarySchool.DAL
{
    public class StudentGateway
    {
        readonly string _connectionString;

        public StudentGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<StudentData>> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<StudentData>(
                    @"select s.StudentId,
                             s.FirstName,
                             s.LastName,
                             s.BirthDate,
                             s.GitHubLogin
                      from iti.vStudent s;" );
            }
        }

        public async Task<Result<StudentData>> FindById( int studentId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                StudentData student = await con.QueryFirstOrDefaultAsync<StudentData>(
                    @"select s.StudentId,
                             s.FirstName,
                             s.LastName,
                             s.BirthDate,
                             s.GitHubLogin
                      from iti.vStudent s
                      where s.StudentId = @StudentId;",
                    new { StudentId = studentId } );

                if( student == null ) return Result.Failure<StudentData>( Status.NotFound, "Student not found." );
                return Result.Success( student );
            }
        }

        public async Task<Result<StudentClassData>> FindStudentClassById( int studentId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                StudentClassData student = await con.QueryFirstOrDefaultAsync<StudentClassData>(
                    @"select s.StudentId,
                             s.FirstName,
                             s.LastName,
                             s.BirthDate,
                             s.ClassId
                      from iti.vStudent s
                      where s.StudentId = @StudentId;",
                    new { StudentId = studentId } );

                if( student == null ) return Result.Failure<StudentClassData>( Status.NotFound, "Student not found." );
                return Result.Success( student );
            }
        }

        public async Task<IEnumerable<FollowedStudentData>> GetByGitHubLogin( IEnumerable<string> logins )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<FollowedStudentData>(
                    @"select s.StudentId,
                             s.FirstName,
                             s.LastName,
                             s.GitHubLogin
                        from iti.vStudent s
                        where s.GitHubLogin in @Logins;",
                    new { Logins = logins } );
            }
        }

        public Task<Result<int>> Create( string firstName, string lastName, DateTime birthDate )
        {
            return Create( firstName, lastName, birthDate, string.Empty );
        }

        public async Task<Result<int>> Create( string firstName, string lastName, DateTime birthDate, string gitHubLogin )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<int>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<int>( Status.BadRequest, "The last name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@FirstName", firstName );
                p.Add( "@LastName", lastName );
                p.Add( "@BirthDate", birthDate );
                p.Add( "@GitHubLogin", gitHubLogin ?? string.Empty );
                p.Add( "@StudentId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sStudentCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "A student with this name already exists." );
                if( status == 2 ) return Result.Failure<int>( Status.BadRequest, "A student with GitHub login already exists." );

                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@StudentId" ) );
            }
        }

        public async Task<Result> Delete( int studentId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@StudentId", studentId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sStudentDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if (status == 1) return Result.Failure( Status.NotFound, "Student not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> Update( int studentId, string firstName, string lastName, DateTime birthDate, string gitHubLogin )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure( Status.BadRequest, "The last name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@StudentId", studentId );
                p.Add( "@FirstName", firstName );
                p.Add( "@LastName", lastName );
                p.Add( "@BirthDate", birthDate );
                p.Add( "@GitHubLogin", gitHubLogin ?? string.Empty );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sStudentUpdate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Student not found." );
                if( status == 2 ) return Result.Failure( Status.BadRequest, "A student with this name already exists." );
                if( status == 3 ) return Result.Failure( Status.BadRequest, "A student with this GitHub login already exists." );

                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }
        }

        public async Task AssignClass( int studentId, int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sAssignClass",
                    new { StudentId = studentId, ClassId = classId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
