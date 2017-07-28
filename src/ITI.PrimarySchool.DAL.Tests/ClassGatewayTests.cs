using NUnit.Framework;

namespace ITI.PrimarySchool.DAL.Tests
{
    [TestFixture]
    public class ClassGatewayTests
    {
        [Test]
        public void can_create_find_update_and_delete_class()
        {
            ClassGateway sut = new ClassGateway( TestHelpers.ConnectionString );
            string name = TestHelpers.RandomTestName();
            string level = TestHelpers.RandomLevel();
            sut.Create( name, level );
            Class c;

            {
                c = sut.FindByName( name );
                CheckClass( c, name, level );
            }

            {
                c = sut.FindById( c.ClassId );
                CheckClass( c, name, level );
            }

            {
                name = TestHelpers.RandomTestName();
                level = TestHelpers.RandomLevel();
                sut.Update( c.ClassId, name, level );

                c = sut.FindById( c.ClassId );
                CheckClass( c, name, level );
            }

            {
                sut.Delete( c.ClassId );
                c = sut.FindById( c.ClassId );
                Assert.That( c, Is.Null );
            }
        }

        [Test]
        public void can_assign_teacher()
        {
            TeacherGateway teacherGateway = new TeacherGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            teacherGateway.Create( firstName, lastName );
            Teacher teacher1 = teacherGateway.FindByName( firstName, lastName );

            ClassGateway sut = new ClassGateway( TestHelpers.ConnectionString );
            string name = TestHelpers.RandomTestName();
            string level = TestHelpers.RandomLevel();
            sut.Create( name, level, teacher1.TeacherId );

            Class c;

            {
                c = sut.FindByName( name );
                CheckClass( c, name, level, teacher1.TeacherId );
            }

            {
                string firstName2 = TestHelpers.RandomTestName();
                string lastName2 = TestHelpers.RandomTestName();
                teacherGateway.Create( firstName2, lastName2 );
                Teacher teacher2 = teacherGateway.FindByName( firstName2, lastName2 );
                sut.AssignTeacher( c.ClassId, teacher2.TeacherId );
                c = sut.FindById( c.ClassId );
                CheckClass( c, name, level, teacher2.TeacherId );

                sut.AssignTeacher( c.ClassId, 0 );
                c = sut.FindById( c.ClassId );
                CheckClass( c, name, level, 0 );

                teacherGateway.Delete( teacher2.TeacherId );
            }

            sut.Delete( c.ClassId );
            teacherGateway.Delete( teacher1.TeacherId );
        }

        void CheckClass( Class c, string name, string level )
        {
            Assert.That( c.Name, Is.EqualTo( name ) );
            Assert.That( c.Level, Is.EqualTo( level ) );
        }

        void CheckClass( Class c, string name, string level, int teacherId )
        {
            CheckClass( c, name, level );
            Assert.That( c.TeacherId, Is.EqualTo( teacherId ) );
        }
    }
}
