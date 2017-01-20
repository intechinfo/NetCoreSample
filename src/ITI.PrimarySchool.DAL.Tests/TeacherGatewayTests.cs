using NUnit.Framework;

namespace ITI.PrimarySchool.DAL.Tests
{
    [TestFixture]
    public class TeacherGatewayTests
    {
        [Test]
        public void can_create_find_update_and_delete_teacher()
        {
            TeacherGateway sut = new TeacherGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            sut.Create( firstName, lastName );
            Teacher teacher;

            {
                teacher = sut.FindByName( firstName, lastName );
                CheckTeacher( teacher, firstName, lastName );
            }

            {
                teacher = sut.FindById( teacher.TeacherId );
                CheckTeacher( teacher, firstName, lastName );
            }

            {
                firstName = TestHelpers.RandomTestName();
                lastName = TestHelpers.RandomTestName();
                sut.Update( teacher.TeacherId, firstName, lastName );

                teacher = sut.FindById( teacher.TeacherId );
                CheckTeacher( teacher, firstName, lastName );
            }

            {
                sut.Delete( teacher.TeacherId );
                teacher = sut.FindById( teacher.TeacherId );
                Assert.That( teacher, Is.Null );
            }
        }

        void CheckTeacher( Teacher teacher, string firstName, string lastName )
        {
            Assert.That( teacher.FirstName, Is.EqualTo( firstName ) );
            Assert.That( teacher.LastName, Is.EqualTo( lastName ) );
        }
    }
}
