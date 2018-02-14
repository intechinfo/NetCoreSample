using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public async Task<IEnumerable<Student>> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<Student>(
                    @"select s.StudentId,
                             s.FirstName,
                             s.LastName,
                             s.BirthDate,
                             s.ClassId,
                             s.ClassName,
                             s.[Level],
                             s.TeacherId,
                             s.TeacherFirstName,
                             s.TeacherLastName,
                             s.GitHubLogin
                      from iti.vStudent s;" );
            }
        }

        public async Task<Student> FindById( int studentId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<Student>(
                    @"select s.StudentId,
                                s.FirstName,
                                s.LastName,
                                s.BirthDate,
                                s.ClassId,
                                s.ClassName,
                                s.[Level],
                                s.TeacherId,
                                s.TeacherFirstName,
                                s.TeacherLastName,
                                s.GitHubLogin
                        from iti.vStudent s
                        where s.StudentId = @StudentId;",
                    new { StudentId = studentId } );
            }
        }

        public async Task<IEnumerable<Student>> GetByGitHubLogin( IEnumerable<string> logins )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<Student>(
                    @"select s.StudentId,
                                s.FirstName,
                                s.LastName,
                                s.BirthDate,
                                s.ClassId,
                                s.ClassName,
                                s.[Level],
                                s.TeacherId,
                                s.TeacherFirstName,
                                s.TeacherLastName,
                                s.GitHubLogin
                        from iti.vStudent s
                        where s.GitHubLogin in @Logins;",
                    new { Logins = logins } );
            }
        }

        public async Task<Student> FindByName( string firstName, string lastName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<Student>(
                    @"select s.StudentId,
                                s.FirstName,
                                s.LastName,
                                s.BirthDate,
                                s.ClassId,
                                s.ClassName,
                                s.[Level],
                                s.TeacherId,
                                s.TeacherFirstName,
                                s.TeacherLastName,
                                s.GitHubLogin
                        from iti.vStudent s
                        where s.firstName = @FirstName and s.lastName = @LastName;",
                    new { FirstName = firstName, LastName = lastName } );
            }
        }

        public async Task<Student> FindByGitHubLogin( string login )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<Student>(
                    @"select s.StudentId,
                                s.FirstName,
                                s.LastName,
                                s.BirthDate,
                                s.ClassId,
                                s.ClassName,
                                s.[Level],
                                s.TeacherId,
                                s.TeacherFirstName,
                                s.TeacherLastName,
                                s.GitHubLogin
                        from iti.vStudent s
                        where s.GitHubLogin = @GitHubLogin;",
                    new { GitHubLogin = login } );
            }
        }

        public Task Create( string firstName, string lastName, DateTime birthDate )
        {
            return Create( firstName, lastName, birthDate, string.Empty );
        }

        public Task Create( string firstName, string lastName, DateTime birthDate, string gitHubLogin )
        {
            return Create( firstName, lastName, birthDate, gitHubLogin, 0 );
        }


        public async Task Create( string firstName, string lastName, DateTime birthDate, string gitHubLogin, int classId )
        {
            gitHubLogin = gitHubLogin ?? string.Empty;
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sStudentCreate",
                    new { FirstName = firstName, LastName = lastName, BirthDate = birthDate, ClassId = classId, GitHubLogin = gitHubLogin },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task Delete( int studentId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sStudentDelete",
                    new { StudentId = studentId },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task Update( int studentId, string firstName, string lastName, DateTime birthDate, string gitHubLogin )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sStudentUpdate",
                    new { StudentId = studentId, FirstName = firstName, LastName = lastName, BirthDate = birthDate, GitHubLogin = gitHubLogin ?? string.Empty },
                    commandType: CommandType.StoredProcedure );
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
    }
}
