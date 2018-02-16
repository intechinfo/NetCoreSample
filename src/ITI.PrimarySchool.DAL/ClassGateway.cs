using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace ITI.PrimarySchool.DAL
{
    public class ClassGateway
    {
        readonly string _connectionString;

        public ClassGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ClassData>> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<ClassData>(
                    @"select c.ClassId,
                             c.Name,
                             c.[Level]
                      from iti.vClass c;" );
            }
        }

        public async Task<Result<ClassData>> FindById( int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                ClassData c = await con.QueryFirstOrDefaultAsync<ClassData>(
                    @"select c.ClassId,
                             c.Name,
                             c.[Level],
                             c.TeacherId,
                             c.TeacherLastName,
                             c.TeacherLastName
                        from iti.vClass c
                        where c.ClassId = @ClassId;",
                    new { ClassId = classId } );

                if( c == null ) return Result.Failure<ClassData>( Status.NotFound, "Class not found." );
                return Result.Success( Status.Ok, c );
            }
        }

        public Task<Result<int>> Create( string name, string level )
        {
            return Create( name, level, 0 );
        }

        public async Task<Result<int>> Create( string name, string level, int teacherId )
        {
            if( !IsNameValid( name ) ) return Result.Failure<int>( Status.BadRequest, "The class name is not valid." );
            if( !IsLevelValid( level ) ) return Result.Failure<int>( Status.BadRequest, "The class level is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@Name", name );
                p.Add( "@Level", level);
                p.Add( "@TeacherId", teacherId );
                p.Add( "@ClassId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sClassCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "A class with this name already exists." );
                if( status == 2 ) return Result.Failure<int>( Status.BadRequest, "Teacher not found." );

                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@ClassId" ) );
            }
        }

        public async Task<IEnumerable<ClassData>> GetNotAssigned()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<ClassData>(
                    @"select c.ClassId,
                             c.Name,
                             c.[Level]
                      from iti.vClass c
                      where c.TeacherId = 0;" );
            }
        }

        public async Task<Result> Delete( int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ClassId", classId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sClassDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Class not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> Update( int classId, string name, string level )
        {
            if( !IsNameValid( name ) ) return Result.Failure<ClassData>( Status.BadRequest, "The class name is not valid." );
            if( !IsLevelValid( level ) ) return Result.Failure<ClassData>( Status.BadRequest, "The class level is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ClassId", classId );
                p.Add( "@Name", name );
                p.Add( "@Level", level );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sClassUpdate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Class not found." );
                if( status == 2 ) return Result.Failure( Status.BadRequest, "A class with this name already exists." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result<AssignedClassData>> AssignedClass( int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                AssignedClassData classData = await con.QueryFirstOrDefaultAsync<AssignedClassData>(
                    @"select c.ClassId, c.[Name] from iti.vTeacherClass c where c.TeacherId = @TeacherId;",
                    new { TeacherId = teacherId } );

                if( classData == null ) return Result.Failure<AssignedClassData>( Status.BadRequest, "Unknown teacher." );
                return Result.Success( classData );
            }
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
