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
            Result<int> result = await classGateway.Create( className1, level1 );
            Assert.That( result.Status, Is.EqualTo( Status.Created ) );
            int class1Id = result.Content;

            StudentGateway sut = new StudentGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            DateTime birthDate = TestHelpers.RandomBirthDate( _random.Next( 5, 10 ) );
            await sut.Create( firstName, lastName, birthDate, string.Empty, class1Id );

            Student student;

            {
                student = await sut.FindByName( firstName, lastName );
                CheckStudent( student, firstName, lastName, birthDate, class1Id );
            }

            {
                string className2 = TestHelpers.RandomTestName();
                string level2 = "CP";
                result = await classGateway.Create( className2, level2 );
                int class2Id = result.Content;
                Assert.That( result.Status, Is.EqualTo( Status.Created ) );
                await sut.AssignClass( student.StudentId, class2Id );
                student = await sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate, class2Id );

                await sut.AssignClass( student.StudentId, 0 );
                student = await sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate, 0 );

                await classGateway.Delete( class2Id );
            }

            await sut.Delete( student.StudentId );
            await classGateway.Delete( class1Id );
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
