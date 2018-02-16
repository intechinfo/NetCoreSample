using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace ITI.PrimarySchool.DAL
{
    public class TeacherGateway
    {
        readonly string _connectionString;

        public TeacherGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TeacherData>> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<TeacherData>( @"select t.TeacherId, t.FirstName, t.LastName from iti.vTeacher t;" );
            }
        }

        public async Task<Result<TeacherData>> FindById( int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                TeacherData teacher = await con.QueryFirstOrDefaultAsync<TeacherData>(
                    @"select t.TeacherId, t.FirstName, t.LastName from iti.vTeacher t where t.TeacherId = @TeacherId;",
                    new { TeacherId = teacherId } );
                if( teacher == null ) return Result.Failure<TeacherData>( Status.NotFound, "Teacher not found." );
                return Result.Success( teacher );
            }
        }

        public async Task<Result<int>> Create( string firstName, string lastName )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<int>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<int>( Status.BadRequest, "The last name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@FirstName", firstName );
                p.Add( "@LastName", lastName );
                p.Add( "@TeacherId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sTeacherCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "A teacher with this name already exists." );

                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@TeacherId" ) );
            }
        }

        public async Task<Result> Delete( int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TeacherId", teacherId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sTeacherDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Teacher not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> Update( int teacherId, string firstName, string lastName )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure( Status.BadRequest, "The last name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TeacherId", teacherId );
                p.Add( "@FirstName", firstName );
                p.Add( "@LastName", lastName );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sTeacherUpdate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Teacher not found." );
                if( status == 2 ) return Result.Failure( Status.BadRequest, "A teacher with this name already exists." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> AssignClass( int teacherId, int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ClassId", classId );
                p.Add( "@TeacherId", teacherId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sAssignTeacher", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.BadRequest, "Unknown teacher." );
                if( status == 2 ) return Result.Failure( Status.BadRequest, "Unknown class." );
                if( status == 3 ) return Result.Failure( Status.BadRequest, "Class already assigned." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
