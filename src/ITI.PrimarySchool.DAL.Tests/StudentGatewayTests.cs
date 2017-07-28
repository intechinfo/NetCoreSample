using System;
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
        public void can_create_find_update_and_delete_student()
        {
            StudentGateway sut = new StudentGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            DateTime birthDate = TestHelpers.RandomBirthDate( _random.Next( 5, 10 ) );
            sut.Create( firstName, lastName, birthDate );
            Student student;

            {
                student = sut.FindByName( firstName, lastName );
                CheckStudent( student, firstName, lastName, birthDate );
            }

            {
                student = sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate );
            }

            {
                firstName = TestHelpers.RandomTestName();
                lastName = TestHelpers.RandomTestName();
                birthDate = TestHelpers.RandomBirthDate( _random.Next( 5, 10 ) );
                sut.Update( student.StudentId, firstName, lastName, birthDate, null );

                student = sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate );
            }

            {
                sut.Delete( student.StudentId );
                student = sut.FindById( student.StudentId );
                Assert.That( student, Is.Null );
            }
        }

        [Test]
        public void can_assign_class()
        {
            ClassGateway classGateway = new ClassGateway( TestHelpers.ConnectionString );
            string className1 = TestHelpers.RandomTestName();
            string level1 = TestHelpers.RandomLevel();
            classGateway.Create( className1, level1 );
            Class c1 = classGateway.FindByName( className1 );

            StudentGateway sut = new StudentGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            DateTime birthDate = TestHelpers.RandomBirthDate( _random.Next( 5, 10 ) );
            sut.Create( firstName, lastName, birthDate, string.Empty, c1.ClassId );

            Student student;

            {
                student = sut.FindByName( firstName, lastName );
                CheckStudent( student, firstName, lastName, birthDate, c1.ClassId );
            }

            {
                string className2 = TestHelpers.RandomTestName();
                string level2 = "CP";
                classGateway.Create( className2, level2 );
                Class c2 = classGateway.FindByName( className2 );
                sut.AssignClass( student.StudentId, c2.ClassId );
                student = sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate, c2.ClassId );

                sut.AssignClass( student.StudentId, 0 );
                student = sut.FindById( student.StudentId );
                CheckStudent( student, firstName, lastName, birthDate, 0 );

                classGateway.Delete( c2.ClassId );
            }

            sut.Delete( student.StudentId );
            classGateway.Delete( c1.ClassId );
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
