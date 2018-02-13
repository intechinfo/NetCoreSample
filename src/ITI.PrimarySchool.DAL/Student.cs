using System;

namespace ITI.PrimarySchool.DAL
{
    public class Student
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public string Level { get; set; }

        public int TeacherId { get; set; }

        public string TeacherFirstName { get; set; }

        public string TeacherLastName { get; set; }

        public string GitHubLogin { get; set; }
    }
}