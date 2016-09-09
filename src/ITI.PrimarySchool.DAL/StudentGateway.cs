using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public IEnumerable<Student> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Student>(
                    @"select s.StudentId,
                             s.FirstName,
                             s.LastName,
                             s.BirthDate,
                             s.ClassId,
                             s.ClassName,
                             s.[Level],
                             s.TeacherId,
                             s.TeacherFirstName,
                             s.TeacherLastName
                      from iti.vStudent s;" );
            }
        }

        public Student FindById( int studentId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Student>(
                        @"select s.StudentId,
                                 s.FirstName,
                                 s.LastName,
                                 s.BirthDate,
                                 s.ClassId,
                                 s.ClassName,
                                 s.[Level],
                                 s.TeacherId,
                                 s.TeacherFirstName,
                                 s.TeacherLastName
                          from iti.vStudent s
                          where s.StudentId = @StudentId;",
                        new { StudentId = studentId } )
                    .FirstOrDefault();
            }
        }

        public Student FindByName( string firstName, string lastName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<Student>(
                        @"select s.StudentId,
                                 s.FirstName,
                                 s.LastName,
                                 s.BirthDate,
                                 s.ClassId,
                                 s.ClassName,
                                 s.[Level],
                                 s.TeacherId,
                                 s.TeacherFirstName,
                                 s.TeacherLastName
                          from iti.vStudent s
                          where s.firstName = @FirstName and s.lastName = @LastName;",
                        new { FirstName = firstName, LastName = lastName } )
                    .FirstOrDefault();
            }
        }

        public void Create( string firstName, string lastName, DateTime birthDate )
        {
            Create( firstName, lastName, birthDate, 0 );
        }


        public void Create( string firstName, string lastName, DateTime birthDate, int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sStudentCreate",
                    new { FirstName = firstName, LastName = lastName, BirthDate = birthDate, ClassId = classId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void Delete( int studentId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sStudentDelete",
                    new { StudentId = studentId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void Update( int studentId, string firstName, string lastName, DateTime birthDate )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sStudentUpdate",
                    new { StudentId = studentId, FirstName = firstName, LastName = lastName, BirthDate = birthDate },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void AssignClass( int studentId, int classId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sAssignClass",
                    new { StudentId = studentId, ClassId = classId },
                    commandType: CommandType.StoredProcedure );
            }
        }
    }
}
