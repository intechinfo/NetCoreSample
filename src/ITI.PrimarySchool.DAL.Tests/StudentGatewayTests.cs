using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.PrimarySchool.DAL.Tests
{
    [TestFixture]
    public class StudentGatewayTests
    {
        readonly Random _random;

        public StudentGatewayTests()
        {
            _random = new Random();
        }

        [Test]
        public async Task can_create_find_update_and_delete_student()
        {
            StudentGateway sut = new StudentGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            DateTime birthDate = TestHelpers.RandomBirthDate( _random.Next( 5, 10 ) );
            Result<int> studentResult = await sut.Create( firstName, lastName, birthDate );
            Assert.That( studentResult.Status, Is.EqualTo( Status.Created ) );
            int studentId = studentResult.Content;
            Result<StudentData> student;

            {
                student = await sut.FindById( studentId );
                CheckStudent( student, firstName, lastName, birthDate );
            }

            {
                firstName = TestHelpers.RandomTestName();
                lastName = TestHelpers.RandomTestName();
                birthDate = TestHelpers.RandomBirthDate( _random.Next( 5, 10 ) );
                Result r = await sut.Update( studentId, firstName, lastName, birthDate, null );
                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );

                student = await sut.FindById( studentId );
                CheckStudent( student, firstName, lastName, birthDate );
            }

            {
                Result r = await sut.Delete( studentId );
                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );
                student = await sut.FindById( studentId );
                Assert.That( student.Status, Is.EqualTo( Status.NotFound ) );
            }
        }

        [Test]
        public async Task can_assign_class()
        {
            ClassGateway classGateway = new ClassGateway( TestHelpers.ConnectionString );
            StudentGateway sut = new StudentGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            DateTime birthDate = TestHelpers.RandomBirthDate( _random.Next( 5, 10 ) );
            Result<int> studentResult = await sut.Create( firstName, lastName, birthDate );
            int studentId = studentResult.Content;

            int classId1;
            {
                string className = TestHelpers.RandomTestName();
                string level = TestHelpers.RandomLevel();
                Result<int> classResult = await classGateway.Create( className, level );
                Assert.That( classResult.Status, Is.EqualTo( Status.Created ) );
                classId1 = classResult.Content;

                await sut.AssignClass( studentId, classId1 );
                Result<StudentClassData> studentClass = await sut.FindStudentClassById( studentId );
                CheckStudent( studentClass, firstName, lastName, birthDate, classId1 );
            }

            {
                string className = TestHelpers.RandomTestName();
                string level = "CP";
                Result<int> classResult = await classGateway.Create( className, level );
                int classId2 = classResult.Content;
                await sut.AssignClass( studentId, classId2 );
                Result<StudentClassData> studentClass = await sut.FindStudentClassById( studentId );
                CheckStudent( studentClass, firstName, lastName, birthDate, classId2 );

                await sut.AssignClass( studentId, 0 );
                studentClass = await sut.FindStudentClassById( studentId );
                CheckStudent( studentClass, firstName, lastName, birthDate, 0 );

                await classGateway.Delete( classId2 );
            }

            await sut.Delete( studentId );
            await classGateway.Delete( classId1 );
        }

        void CheckStudent( Result<StudentData> student, string firstName, string lastName, DateTime birthDate )
        {
            Assert.That( student.Status, Is.EqualTo( Status.Ok ) );
            Assert.That( student.Content.FirstName, Is.EqualTo( firstName ) );
            Assert.That( student.Content.LastName, Is.EqualTo( lastName ) );
            Assert.That( student.Content.BirthDate, Is.EqualTo( birthDate ) );
        }

        void CheckStudent( Result<StudentClassData> studentClass, string firstName, string lastName, DateTime birthDate, int classId )
        {
            Assert.That( studentClass.Status, Is.EqualTo( Status.Ok ) );
            Assert.That( studentClass.Content.FirstName, Is.EqualTo( firstName ) );
            Assert.That( studentClass.Content.LastName, Is.EqualTo( lastName ) );
            Assert.That( studentClass.Content.BirthDate, Is.EqualTo( birthDate ) );
            Assert.That( studentClass.Content.ClassId, Is.EqualTo( classId ) );
        }
    }
}
