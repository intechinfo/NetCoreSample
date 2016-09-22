using System;
using System.Collections.Generic;
using ITI.PrimarySchool.DAL;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class TeacherService
    {
        readonly TeacherGateway _teacherGateway;
        readonly ClassGateway _classGateway;

        public TeacherService( TeacherGateway teacherGateway, ClassGateway classGateway )
        {
            _teacherGateway = teacherGateway;
            _classGateway = classGateway;
        }

        public Result<Teacher> CreateTeacher( string firstName, string lastName )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Teacher>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Teacher>( Status.BadRequest, "The last name is not valid." );
            if( _teacherGateway.FindByName( firstName, lastName ) != null ) return Result.Failure<Teacher>( Status.BadRequest, "A teacher with this name already exists." );

            _teacherGateway.Create( firstName, lastName );
            Teacher teacher = _teacherGateway.FindByName( firstName, lastName );
            return Result.Success( Status.Created, teacher );
        }

        public Result<Teacher> UpdateTeacher( int teacherId, string firstName, string lastName )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Teacher>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Teacher>( Status.BadRequest, "The last name is not valid." );
            Teacher teacher;
            if( ( teacher = _teacherGateway.FindById( teacherId ) ) == null )
            {
                return Result.Failure<Teacher>( Status.NotFound, "Teacher not found." );
            }

            {
                Teacher t = _teacherGateway.FindByName( firstName, lastName );
                if( t != null && t.TeacherId != teacher.TeacherId ) return Result.Failure<Teacher>( Status.BadRequest, "A teacher with this name already exists." );
            }

            _teacherGateway.Update( teacherId, firstName, lastName );
            teacher = _teacherGateway.FindById( teacherId );
            return Result.Success( Status.Ok, teacher );
        }

        public Result<Teacher> GetById( int id )
        {
            Teacher teacher;
            if( ( teacher = _teacherGateway.FindById( id ) ) == null ) return Result.Failure<Teacher>( Status.NotFound, "Teacher not found." );
            return Result.Success( Status.Ok, teacher );
        }

        public Result<int> Delete( int teacherId )
        {
            if( _teacherGateway.FindById( teacherId ) == null ) return Result.Failure<int>( Status.NotFound, "Teacher not found." );
            _teacherGateway.Delete( teacherId );
            return Result.Success( Status.Ok, teacherId );
        }

        public Result<IEnumerable<Teacher>> GetAll()
        {
            return Result.Success( Status.Ok, _teacherGateway.GetAll() );
        }

        public Result AssignClass( int teacherId, int classId )
        {
            Class c = null;
            if( _teacherGateway.FindById( teacherId ) == null ) return Result.Failure( Status.BadRequest, "Unknown teacher." );
            if( classId != 0 && ( c = _classGateway.FindById( classId ) ) == null ) return Result.Failure( Status.BadRequest, "Unknown class." );
            if( c != null && c.TeacherId != 0 && c.TeacherId != teacherId ) return Result.Failure( Status.BadRequest, "Class already assigned." );
            _teacherGateway.AssignClass( teacherId, classId );
            return Result.Success( Status.Ok );
        }

        public Result<Class> AssignedClass( int teacherId )
        {
            if( _teacherGateway.FindById( teacherId ) == null ) return Result.Failure<Class>( Status.BadRequest, "Unknown teacher." );
            Class c = _classGateway.FindByTeacherId( teacherId );
            if( c == null ) c = new Class();

            return Result.Success( Status.Ok, c );
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
