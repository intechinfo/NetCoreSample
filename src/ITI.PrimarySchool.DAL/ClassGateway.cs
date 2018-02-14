using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public async Task<IEnumerable<Class>> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<Class>(
                    @"select c.ClassId,
                             c.Name,
                             c.[Level],
                             c.TeacherId,
                             c.TeacherLastName,
                             c.TeacherLastName
                      from iti.vClass c;" );
            }
        }

        public async Task<Class> FindById( int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<Class>(
                    @"select c.ClassId,
                                c.Name,
                                c.[Level],
                                c.TeacherId,
                                c.TeacherLastName,
                                c.TeacherLastName
                        from iti.vClass c
                        where c.ClassId = @ClassId;",
                    new { ClassId = classId } );
            }
        }

        public async Task<Class> FindByName( string name )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<Class>(
                    @"select c.ClassId,
                                c.Name,
                                c.[Level],
                                c.TeacherId,
                                c.TeacherLastName,
                                c.TeacherLastName
                        from iti.vClass c
                        where c.Name = @Name;",
                    new { Name = name } );
            }
        }

        public Task Create( string name, string level )
        {
            return Create( name, level, 0 );
        }


        public async Task Create( string name, string level, int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sClassCreate",
                    new { Name = name, Level = level, TeacherId = teacherId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task<IEnumerable<Class>> GetNotAssigned()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<Class>(
                    @"select c.ClassId,
                             c.Name,
                             c.[Level]
                      from iti.vClass c
                      where c.TeacherId = 0;" );
            }
        }

        public async Task Delete( int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sClassDelete",
                    new { ClassId = classId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task Update( int classId, string name, string level )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sClassUpdate",
                    new { ClassId = classId, Name = name, Level = level },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task<Class> FindByTeacherId( int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<Class>(
                    @"select c.ClassId,
                                c.Name,
                                c.[Level],
                                c.TeacherId,
                                c.TeacherLastName,
                                c.TeacherLastName
                        from iti.vClass c
                        where c.TeacherId = @TeacherId;",
                    new { TeacherId = teacherId } );
            }
        }

        public async Task AssignTeacher( int classId, int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sAssignTeacher",
                    new { ClassId = classId, TeacherId = teacherId },
                    commandType: CommandType.StoredProcedure );
            }
        }
    }
}
