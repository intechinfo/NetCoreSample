using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public async Task<IEnumerable<Teacher>> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<Teacher>( @"select t.TeacherId, t.FirstName, t.LastName from iti.vTeacher t;" );
            }
        }

        public async Task<Teacher> FindById( int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<Teacher>(
                    @"select t.TeacherId, t.FirstName, t.LastName from iti.vTeacher t where t.TeacherId = @TeacherId;",
                    new { TeacherId = teacherId } );
            }
        }

        public async Task<Teacher> FindByName( string firstName, string lastName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<Teacher>(
                    @"select t.TeacherId,
                                t.FirstName,
                                t.LastName
                        from iti.vTeacher t
                        where t.FirstName = @FirstName and t.LastName = @LastName;",
                    new { FirstName = firstName, LastName = lastName } );
            }
        }


        public async Task Create( string firstName, string lastName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sTeacherCreate",
                    new { FirstName = firstName, LastName = lastName },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task Delete( int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sTeacherDelete",
                    new { TeacherId = teacherId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task Update( int teacherId, string firstName, string lastName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sTeacherUpdate",
                    new { TeacherId = teacherId, FirstName = firstName, LastName = lastName },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task AssignClass( int teacherId, int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sAssignTeacher",
                    new { TeacherId = teacherId, ClassId = classId },
                    commandType: CommandType.StoredProcedure );
            }
        }
    }
}
