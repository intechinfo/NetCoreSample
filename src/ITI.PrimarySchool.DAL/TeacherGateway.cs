using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public IEnumerable<Teacher> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Teacher>( @"select t.TeacherId, t.FirstName, t.LastName from iti.vTeacher t;" );
            }
        }

        public Teacher FindById( int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Teacher>(
                        @"select t.TeacherId, t.FirstName, t.LastName from iti.vTeacher t where t.TeacherId = @TeacherId;",
                        new { TeacherId = teacherId } )
                    .FirstOrDefault();
            }
        }

        public Teacher FindByName( string firstName, string lastName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Teacher>(
                        @"select t.TeacherId,
                                 t.FirstName,
                                 t.LastName
                          from iti.vTeacher t
                          where t.FirstName = @FirstName and t.LastName = @LastName;",
                        new { FirstName = firstName, LastName = lastName } )
                    .FirstOrDefault();
            }
        }


        public void Create( string firstName, string lastName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sTeacherCreate",
                    new { FirstName = firstName, LastName = lastName },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void Delete( int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sTeacherDelete",
                    new { TeacherId = teacherId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void Update( int teacherId, string firstName, string lastName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sTeacherUpdate",
                    new { TeacherId = teacherId, FirstName = firstName, LastName = lastName },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void AssignClass( int teacherId, int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sAssignTeacher",
                    new { TeacherId = teacherId, ClassId = classId },
                    commandType: CommandType.StoredProcedure );
            }
        }
    }
}
