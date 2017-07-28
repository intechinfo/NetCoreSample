using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public IEnumerable<Class> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Class>(
                    @"select c.ClassId,
                             c.Name,
                             c.[Level],
                             c.TeacherId,
                             c.TeacherLastName,
                             c.TeacherLastName
                      from iti.vClass c;" );
            }
        }

        public Class FindById( int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Class>(
                        @"select c.ClassId,
                                 c.Name,
                                 c.[Level],
                                 c.TeacherId,
                                 c.TeacherLastName,
                                 c.TeacherLastName
                          from iti.vClass c
                          where c.ClassId = @ClassId;",
                        new { ClassId = classId } )
                    .FirstOrDefault();
            }
        }

        public Class FindByName( string name )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Class>(
                        @"select c.ClassId,
                                 c.Name,
                                 c.[Level],
                                 c.TeacherId,
                                 c.TeacherLastName,
                                 c.TeacherLastName
                          from iti.vClass c
                          where c.Name = @Name;",
                        new { Name = name } )
                    .FirstOrDefault();
            }
        }

        public void Create( string name, string level )
        {
            Create( name, level, 0 );
        }


        public void Create( string name, string level, int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sClassCreate",
                    new { Name = name, Level = level, TeacherId = teacherId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public IEnumerable<Class> GetNotAssigned()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Class>(
                    @"select c.ClassId,
                             c.Name,
                             c.[Level]
                      from iti.vClass c
                      where c.TeacherId = 0;" );
            }
        }

        public void Delete( int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sClassDelete",
                    new { ClassId = classId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void Update( int classId, string name, string level )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sClassUpdate",
                    new { ClassId = classId, Name = name, Level = level },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public Class FindByTeacherId( int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Class>(
                        @"select c.ClassId,
                                 c.Name,
                                 c.[Level],
                                 c.TeacherId,
                                 c.TeacherLastName,
                                 c.TeacherLastName
                          from iti.vClass c
                          where c.TeacherId = @TeacherId;",
                        new { TeacherId = teacherId } )
                    .FirstOrDefault();
            }
        }

        public void AssignTeacher( int classId, int teacherId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sAssignTeacher",
                    new { ClassId = classId,TeacherId = teacherId },
                    commandType: CommandType.StoredProcedure );
            }
        }
    }
}
