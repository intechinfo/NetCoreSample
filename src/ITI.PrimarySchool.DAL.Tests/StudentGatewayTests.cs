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
            await sut.Create( firstName, lastName, birthDate );
            Student student;

            {
                student = await sut.FindByName( firstName, lastName );
                CheckStudent( student, firstName, lastName, birthDate );
            }

            {
                student = await sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate );
            }

            {
                firstName = TestHelpers.RandomTestName();
                lastName = TestHelpers.RandomTestName();
                birthDate = TestHelpers.RandomBirthDate( _random.Next( 5, 10 ) );
                await sut.Update( student.StudentId, firstName, lastName, birthDate, null );

                student = await sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate );
            }

            {
                await sut.Delete( student.StudentId );
                student = await sut.FindById( student.StudentId );
                Assert.That( student, Is.Null );
            }
        }

        [Test]
        public async Task can_assign_class()
        {
            ClassGateway classGateway = new ClassGateway( TestHelpers.ConnectionString );
            string className1 = TestHelpers.RandomTestName();
            string level1 = TestHelpers.RandomLevel();
            await classGateway.Create( className1, level1 );
            Class c1 = await classGateway.FindByName( className1 );

            StudentGateway sut = new StudentGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            DateTime birthDate = TestHelpers.RandomBirthDate( _random.Next( 5, 10 ) );
            await sut.Create( firstName, lastName, birthDate, string.Empty, c1.ClassId );

            Student student;

            {
                student = await sut.FindByName( firstName, lastName );
                CheckStudent( student, firstName, lastName, birthDate, c1.ClassId );
            }

            {
                string className2 = TestHelpers.RandomTestName();
                string level2 = "CP";
                await classGateway.Create( className2, level2 );
                Class c2 = await classGateway.FindByName( className2 );
                await sut.AssignClass( student.StudentId, c2.ClassId );
                student = await sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate, c2.ClassId );

                await sut.AssignClass( student.StudentId, 0 );
                student = await sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate, 0 );

                await classGateway.Delete( c2.ClassId );
            }

            await sut.Delete( student.StudentId );
            await classGateway.Delete( c1.ClassId );
        }

        void CheckStudent( Student student, string firstName, string lastName, DateTime birthDate )
        {
            Assert.That( student.FirstName, Is.EqualTo( firstName ) );
            Assert.That( student.LastName, Is.EqualTo( lastName ) );
            Assert.That( student.BirthDate, Is.EqualTo( birthDate ) );
        }

        void CheckStudent( Student student, string firstName, string lastName, DateTime birthDate, int classId )
        {
            CheckStudent( student, firstName, lastName, birthDate );
            Assert.That( student.ClassId, Is.EqualTo( classId ) );
        }
    }
}
